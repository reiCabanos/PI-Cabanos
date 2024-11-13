using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GerenciadorJogo : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerLives = 3;

    [Header("Player Aura Settings")]
    public List<Color> playerColors; // Lista de cores para os jogadores


    [Header("UI References")]
    public Button startGameButton;
    public Button resetGameButton;
    public Button retryButton;          // Novo botão "Jogar Novamente"
    public GameObject startPanel;       // Painel que contém os botões

    public TextMeshProUGUI player1LivesText;
    public Image player1ColorIndicator;
    public TextMeshProUGUI player2LivesText;
    public Image player2ColorIndicator;
    public TextMeshProUGUI gameStateText;
    public TextMeshProUGUI timerText;
    public GameObject endGamePanel;  // Painel de fundo para vitória/derrota
    public TextMeshProUGUI endGameMessage;  // Texto para mensagem de vitória/empate
    public GameObject pausePanel; // Painel de pausa
    public Button backButton;      // Botão "Voltar ao Menu"
    public Button resumeButton;    // Botão "Continuar Jogo"
    private bool isPaused = false;
    private bool wasStartPanelVisible = false; // Variável para rastrear o estado do startPanel antes da pausa

    [Header("Game Settings")]
    public float initialProblemDuration;
    public float minimumProblemDuration;
    public float timeDecreaseRate = 0.2f;
    public float singlePlayerWinTime = 120.0f;

    private GerenciadorMiniGame miniGameManager;
    private BlockSpawner blockSpawner;
    private List<PlayerData> players = new List<PlayerData>();
    private bool gameInProgress = false;
    private float gameTimer = 0.0f;

    private class PlayerData
    {
        public PlayerInput playerInput;
        public int lives;

        public PlayerData(PlayerInput input, int lives)
        {
            playerInput = input;
            this.lives = lives;
        }
    }

    void Update()
    {
        // Verifica se a tecla ESC ou o botão START do joystick foram pressionados usando o novo sistema de Input
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame)
        {
            TogglePauseGame();
        }
    }

    void Start()
    {
        // Configura os botões para as ações correspondentes
        backButton.onClick.AddListener(ReturnToMenu);
        resumeButton.onClick.AddListener(ResumeGame);
        retryButton.onClick.AddListener(RetryGame); // Configura o botão "Jogar Novamente"
        


        // Inicialmente oculta o painel
        startPanel.SetActive(false);

        // Exibe o painel com animação ao iniciar
        ShowStartPanel();

        // Inicialmente oculta o painel de pausa
        pausePanel.SetActive(false);
        retryButton.gameObject.SetActive(false);

        miniGameManager = FindObjectOfType<GerenciadorMiniGame>();
        blockSpawner = Camera.main?.GetComponent<BlockSpawner>();

        SetupInitialUI();

        // Registro dos eventos de entrada e saída de jogadores
        var playerInputManager = PlayerInputManager.instance;
        if (playerInputManager != null)
        {
            playerInputManager.onPlayerJoined += OnPlayerJoined;
            playerInputManager.onPlayerLeft += OnPlayerLeft;
        }

        // Configura o botão de iniciar para chamar o método OnStartButtonClicked
        startGameButton.onClick.AddListener(OnStartButtonClicked);
        // Seleciona automaticamente o botão "Start Game" ao carregar a tela inicial
        EventSystem.current.SetSelectedGameObject(startGameButton.gameObject);


        // Configura o botão de reset para chamar o método ResetGame
        resetGameButton.onClick.AddListener(ResetGame);

    }

   

    private void SetupInitialUI()
    {
        startGameButton.interactable = false;
        UpdateGameStateText("Aguardando jogadores...");
        UpdateLivesDisplay();
        UpdateTimerDisplay();
    }

    // Método chamado quando um jogador se junta à partida
    // Método chamado quando um jogador se junta à partida
    // Método chamado quando um jogador se junta à partida
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        var newPlayer = new PlayerData(playerInput, playerLives);
        players.Add(newPlayer);
        UpdateGameStateText($"Jogador {players.Count} entrou! ({players.Count}/{PlayerInputManager.instance.maxPlayerCount})");

        // Atribuir uma cor ao jogador recém-entrado
        AtribuirCorAoJogador(playerInput.gameObject, players.Count - 1);

        // Ativa o botão iniciar se houver 1 jogador e o jogo ainda não tiver começado
        if (players.Count == 1 && !gameInProgress)
        {
            startGameButton.interactable = true;
        }

        // Inicia o jogo automaticamente se houver exatamente 2 jogadores e o jogo ainda não começou
        if (players.Count == 2 && !gameInProgress)
        {
            IniciarJogo(players.ConvertAll(p => p.playerInput));
        }
    }



    // Método chamado quando um jogador sai da partida
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        var playerToRemove = players.Find(p => p.playerInput == playerInput);
        if (playerToRemove != null)
        {
            players.Remove(playerToRemove);
            UpdateGameStateText($"Jogador saiu! ({players.Count}/{PlayerInputManager.instance.maxPlayerCount})");

            // Verifica se o jogo precisa terminar se todos os jogadores saírem
            if (players.Count == 0 && gameInProgress)
            {
                EndGame(-1); // Empate
            }
        }

        // Atualiza a interatividade do botão "Iniciar" dependendo do número de jogadores
        startGameButton.interactable = players.Count == 1 && !gameInProgress;
    }

    // Método auxiliar para iniciar o jogo ao clicar no botão
    public void OnStartButtonClicked()
    {
        Debug.Log("Botão de iniciar clicado! Verificando condições para iniciar o jogo.");
        Debug.Log("Número de jogadores presentes: " + players.Count);

        if (players.Count > 0 && !gameInProgress)
        {
            IniciarJogo(players.ConvertAll(p => p.playerInput));
        }
        else
        {
            Debug.Log("Condições para iniciar o jogo não foram atendidas.");
            Debug.Log("gameInProgress: " + gameInProgress);
        }
        // Oculta o painel com animação
        HideStartPanel();
        gameInProgress = true;
        endGamePanel.SetActive(false); // Garante que o painel de fim de jogo esteja oculto no início da partida
        startGameButton.gameObject.SetActive(false);
    }


    // Inicia o jogo quando o número de jogadores está completo ou quando o botão "Iniciar" é pressionado

    public void IniciarJogo(List<PlayerInput> playerInputs)
    {
        if (gameInProgress) return;

        Debug.Log("Iniciando o jogo com " + playerInputs.Count + " jogadores.");

        HideStartPanel();


        players.Clear();

        foreach (var playerInput in playerInputs)
        {
            players.Add(new PlayerData(playerInput, playerLives));
        }

        gameInProgress = true;
        PlayerInputManager.instance.DisableJoining(); // Impede novos jogadores de entrar
        UpdateGameStateText("Jogo em andamento!");
        startGameButton.interactable = false;
        resetGameButton.interactable = false;

        // Esconde o painel de fim de jogo
        OcultarEndGamePanel();

        // Ativa miniGameManager e blockSpawner para iniciar o jogo
        if (miniGameManager != null)
        {
            miniGameManager.enabled = true;
            miniGameManager.StartGame();
            Debug.Log("MiniGameManager iniciado.");
        }

        if (blockSpawner != null)
        {
            blockSpawner.enabled = true;
            blockSpawner.StartGame();
            Debug.Log("BlockSpawner iniciado.");
        }

        StartCoroutine(DecreaseProblemDuration());

        if (players.Count == 1)
        {
            gameTimer = singlePlayerWinTime;
            StartCoroutine(GameTimerCountdown());
        }

        Debug.Log("Jogo iniciado com sucesso.");
        UpdateLivesDisplay();
    }



    // Notifica que um jogador foi atingido, chamado pelo ManipuladorDeColisaoJogador
    public void OnPlayerHitTrigger(int playerID)
    {
        if (gameInProgress && playerID > 0 && playerID <= players.Count)
        {
            PlayerData player = players[playerID - 1];
            player.lives--;
            UpdateLivesDisplay();

            if (player.lives <= 0)
            {
                EliminatePlayer(player);
            }
        }
    }

    private void EliminatePlayer(PlayerData player)
    {
        Debug.Log($"Jogador {players.IndexOf(player) + 1} foi eliminado.");
        player.playerInput.DeactivateInput();
        OnPlayerLeft(player.playerInput); // Chama OnPlayerLeft para atualizar a lista
        CheckGameEnd();
    }

    private void CheckGameEnd()
    {
        if (players.Count == 1 && gameInProgress)
        {
            EndGame(players[0].playerInput.playerIndex + 1);
        }
        else if (players.Count == 0)
        {
            EndGame(-1); // Empate
        }
    }


    private void EndGame(int winnerID)
    {
        gameInProgress = false;
        string endMessage;

        if (winnerID > 0)
        {
            endMessage = $"Jogador {winnerID} venceu! Parabéns!";
        }
        else
        {
            endMessage = "Empate!";
        }

        endGamePanel.SetActive(true);
        endGamePanel.transform.localScale = Vector3.zero;
        endGamePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);


        // Atualiza o texto do painel de fim de jogo
        if (endGameMessage != null)
        {
            endGameMessage.text = endMessage;
        }
        // Exibe o painel de início novamente após a partida terminar
        ShowStartPanel();


        // Exibe o painel de fim de jogo com animação
        MostrarEndGamePanel();

        miniGameManager.enabled = false;
        blockSpawner.enabled = false;

        foreach (var player in players)
        {
            player.playerInput.DeactivateInput();
        }

        PlayerInputManager.instance.EnableJoining(); // Permite novos jogadores para a próxima partida
        startGameButton.interactable = true;
        resetGameButton.interactable = true;

        Debug.Log("Jogo finalizado.");
        // Ativa o botão "Jogar Novamente" para o final da partida
        retryButton.gameObject.SetActive(true);
        
        // Define o retryButton como o botão selecionado automaticamente
        EventSystem.current.SetSelectedGameObject(retryButton.gameObject);


    }

    private IEnumerator GameTimerCountdown()
    {
        while (gameInProgress && gameTimer > 0)
        {
            yield return new WaitForSeconds(1f);
            gameTimer--;
            UpdateTimerDisplay();

            if (players.Count == 1 && gameTimer <= 0)
            {
                EndGame(players[0].playerInput.playerIndex + 1);
                yield break;
            }
        }
    }

    private void UpdateLivesDisplay()
    {
        player1LivesText.text = players.Count > 0 ? $"P1 Vidas: {players[0].lives}" : "P1 Vidas: 0";
        player2LivesText.text = players.Count > 1 ? $"P2 Vidas: {players[1].lives}" : "P2 Vidas: 0";

     
    }


    private void UpdateGameStateText(string message)
    {
        if (gameStateText != null)
            gameStateText.text = message;
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = $"Tempo restante: {gameTimer:F0} segundos";
    }

    private IEnumerator DecreaseProblemDuration()
    {
        while (gameInProgress && miniGameManager.problemDuration > minimumProblemDuration)
        {
            yield return new WaitForSeconds(10f);
            miniGameManager.problemDuration = Mathf.Max(
                miniGameManager.problemDuration - timeDecreaseRate,
                minimumProblemDuration
            );
        }
    }


    public void ResetGame()
    {
        // 1. Finaliza o jogo atual e redefine variáveis globais
        gameInProgress = false;
        gameTimer = singlePlayerWinTime;

        // 2. Desativa o input de todos os jogadores atuais antes de limpar a lista
        foreach (var player in players)
        {
            player.playerInput.DeactivateInput();
        }

        // 3. Limpa a lista de jogadores para redefini-los
        players.Clear();

        // 4. Permite novos jogadores e redefine o PlayerInputManager
        PlayerInputManager.instance.EnableJoining();

        // 5. Verifica e adiciona os jogadores que já estavam na cena
        VerificarJogadoresExistentes();

        // 6. Reseta o miniGameManager e o blockSpawner sem ativá-los ainda
        if (miniGameManager != null)
        {
            miniGameManager.problemDuration = initialProblemDuration;
            miniGameManager.enabled = false;
        }

        if (blockSpawner != null)
        {
            blockSpawner.InitializeBlocks();
            blockSpawner.enabled = false;
        }

        // 7. Reseta a UI e define o estado inicial do jogo
        UpdateGameStateText("Pressione 'Iniciar' para começar");
        UpdateLivesDisplay();
        UpdateTimerDisplay();

        // 8. Habilita o botão de iniciar se houver jogadores na lista e desativa o botão de reset enquanto o jogo não começou
        startGameButton.interactable = players.Count > 0;
        resetGameButton.interactable = true;

        // 9. Animação de aparecimento do botão reset e painel
        MostrarEndGamePanel();

        Debug.Log("Jogo resetado e pronto para uma nova partida.");
    }


    private void VerificarJogadoresExistentes()
    {
        // Verifica se há jogadores instanciados no início do jogo
        foreach (var playerInput in FindObjectsOfType<PlayerInput>())
        {
            // Se o jogador ainda não está na lista, adiciona e reativa
            if (!players.Exists(p => p.playerInput == playerInput))
            {
                players.Add(new PlayerData(playerInput, playerLives));
                Debug.Log("Jogador reconhecido e adicionado de volta: " + playerInput.playerIndex);
            }

            // Reativa o input do jogador para garantir que ele pode jogar
            playerInput.ActivateInput();
            playerInput.SwitchCurrentActionMap("Player");  // Certifique-se de que o action map correto está ativo
            Debug.Log($"Input do jogador {playerInput.playerIndex} reativado.");
        }
    }



    private void ReiniciarJogador(PlayerData player)
    {
        // Restaura o número de vidas do jogador
        player.lives = playerLives;

        // Reativa o input do jogador para que ele possa se mover novamente
        if (player.playerInput != null)
        {
            player.playerInput.ActivateInput();  // Reativa o controle do jogador
            player.playerInput.SwitchCurrentActionMap("Player");  // Certifique-se de que o action map correto está ativo
        }

        // Reposiciona o jogador ao ponto de spawn inicial (se necessário)
        Transform playerTransform = player.playerInput.transform;
        if (playerTransform != null)
        {
            playerTransform.position = Vector3.zero;  // Ajuste conforme a posição inicial desejada
            playerTransform.rotation = Quaternion.identity;
        }

        Debug.Log($"Jogador {players.IndexOf(player) + 1} reiniciado e pronto para jogar.");
    }

    private void MostrarEndGamePanel()
    {
        // Define a escala inicial do painel e dos botões como zero (invisível)
        endGamePanel.transform.localScale = Vector3.zero;
        startGameButton.transform.localScale = Vector3.zero;
        resetGameButton.transform.localScale = Vector3.zero;

        // Define o painel como ativo para que ele seja visível
        endGamePanel.SetActive(true);

        // Anima o painel e os botões para aparecerem gradualmente no centro da tela
        endGamePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Painel cresce até o tamanho normal
        startGameButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f); // Botão Iniciar aparece logo após
        resetGameButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.4f); // Botão Reset aparece logo depois
    }

    private void OcultarEndGamePanel()
    {
        // Anima o painel para diminuir de volta até desaparecer
        endGamePanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            endGamePanel.SetActive(false); // Após a animação, torna o painel invisível
        });
    }

    // Método para atribuir uma cor única ao jogador, criando uma luz de aura ao redor do personagem
    private void AtribuirCorAoJogador(GameObject playerObject, int playerIndex)
    {
        // Verifica se o índice do jogador está dentro da lista de cores
        if (playerIndex < playerColors.Count)
        {
            Color playerColor = playerColors[playerIndex];
            playerColor.a = 1.0f;

            // Aplica a cor à interface (indicadores UI)
            if (playerIndex == 0 && player1ColorIndicator != null)
            {
                player1ColorIndicator.color = playerColor;
            }
            else if (playerIndex == 1 && player2ColorIndicator != null)
            {
                player2ColorIndicator.color = playerColor;
            }

            // Aplica a cor à luz de aura do jogador
            Light auraLight = playerObject.GetComponentInChildren<Light>();
            if (auraLight == null)
            {
                // Cria a luz se ainda não existir
                GameObject auraLightObj = new GameObject("AuraLight");
                auraLightObj.transform.SetParent(playerObject.transform);
                auraLightObj.transform.localPosition = Vector3.zero;

                auraLight = auraLightObj.AddComponent<Light>();
                auraLight.type = LightType.Point;
                auraLight.range = 5.0f;
                auraLight.intensity = 1.5f;
            }

            auraLight.color = playerColor;
            Debug.Log($"Atribuindo a cor {playerColor} ao jogador {playerIndex + 1}");
        }
        else
        {
            Debug.LogWarning("Não há cores suficientes na lista de cores para todos os jogadores.");
        }
    }



    private void TogglePauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    // Método para pausar o jogo e exibir o painel de pausa com animações
    private void PauseGame()
    {
        Time.timeScale = 0; // Pausa o tempo do jogo
        pausePanel.SetActive(true);
        // Salva o estado do startPanel e o desativa se estiver visível
        wasStartPanelVisible = startPanel.activeSelf;
        if (wasStartPanelVisible)
        {
            startPanel.SetActive(false);
        }

        pausePanel.transform.localScale = Vector3.zero; // Define o tamanho inicial do painel
        pausePanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);

        backButton.transform.localScale = Vector3.zero;
        resumeButton.transform.localScale = Vector3.zero;

        backButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f).SetUpdate(true);
        resumeButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.4f).SetUpdate(true);
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);

    }

    // Método para despausar o jogo e ocultar o painel de pausa com animações
    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // Retoma o tempo do jogo
        if (wasStartPanelVisible)
        {
            startPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }

        // Anima o painel para encolher até desaparecer
        pausePanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            pausePanel.SetActive(false); // Após a animação, torna o painel invisível
        });
    }

    // Retorna ao menu principal
    private void ReturnToMenu()
    {
        Time.timeScale = 1; // Garante que o tempo seja retomado
        SceneManager.LoadScene("Menu");
    }

    private void ShowStartPanel()
    {
        startPanel.SetActive(true);

        // Configura a escala inicial do painel e dos botões para zero
        startPanel.transform.localScale = Vector3.zero;
        startGameButton.transform.localScale = Vector3.zero;
        resetGameButton.transform.localScale = Vector3.zero;
        retryButton.transform.localScale = Vector3.zero;


        // Anima o painel para aparecer com efeito de "crescimento"
        startPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        // Animação dos botões dentro do painel
        startGameButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f);
        resetGameButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.4f);
        retryButton.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(0.6f);


        // Seleciona automaticamente o botão "Start Game" ao exibir o painel
        EventSystem.current.SetSelectedGameObject(startGameButton.gameObject);
    }

    private void HideStartPanel()
    {
        startPanel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            startPanel.SetActive(false); // Torna o painel invisível após a animação
        });
    }

    private void RetryGame()
    {
        ResetGame();           // Reseta o jogo
        OnStartButtonClicked(); // Inicia o jogo novamente
    }

}


