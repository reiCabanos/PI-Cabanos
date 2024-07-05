using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogoPontos : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _radius;
    public bool _scoreMode;
    public PlayerMove _playerMove;
    public GameObject _p;
    public GameObject _p2;
    void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _p2.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        FindThePlayer();

    }

    private void FindThePlayer()
    {
        if (!_scoreMode)
        {
            Collider[] _coinColl = Physics.OverlapSphere(transform.position, _radius);

            foreach (var c in _coinColl)
            {
                if (c.CompareTag("Player"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, c.transform.position
                        + new Vector3(0f, 0.5f, 0f), Time.deltaTime * 50f);

                }

            }
        }
        /*else
        {
            ConvertToScore();

        }
        */


    }
   /* private void ConvertToScore()
    {

        transform.position = Vector3.MoveTowards(transform.position, _playerMove._coinNextPos.position, Time.deltaTime * 100f);
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * 5f);
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //_playerMove._scoreCounter++;
            //_playerMove._coinCounterTex.text = _playerMove._scoreCounter.ToString();

            GetComponent<Collider>().enabled = false;
            Invoke("InActiveCoin", 0.5f);
            _p.gameObject.SetActive(true);
            _p2.gameObject.SetActive(false);



        }
    }
    void InActiveCoin()
    {
        gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }
    
}
