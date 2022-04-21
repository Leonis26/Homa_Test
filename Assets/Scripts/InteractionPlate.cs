using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.AI;

public class InteractionPlate : MonoBehaviour
{
    public bool Activable { get; private set; } = true;
    [SerializeField] float m_activatedHeight;
    public float ActivatedHeight => m_activatedHeight;
    [SerializeField] Transform m_goToInteract, m_interactable, m_plate;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Activable)
        {
            Activable = false;
            m_plate.DOLocalMoveY(ActivatedHeight, .1f);
            StartCoroutine(WaitAndDoInteraction(other.GetComponent<PlayerController>()));
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
    }*/

    void Interaction()
    {

    }

    IEnumerator WaitAndDoInteraction(PlayerController _player)
    {
        //yield return new WaitForSeconds(.1f);
        _player.State = PersonController.STATE.INTERACTING;
        NavMesh.SamplePosition(m_goToInteract.position, out NavMeshHit hit, .1f, NavMesh.AllAreas);
        var playerT = _player.transform;
        //var playerA = _player.Agent;
        while ((m_goToInteract.position - playerT.position).magnitude > .05f) //new Vector2(playerT.position.x, playerT.position.z) != new Vector2(m_goToInteract.position.x, m_goToInteract.position.z))
        {
            _player.DoMove(m_goToInteract.position - playerT.position);
            yield return new WaitForEndOfFrame();
        }
        playerT.position = hit.position;
        //_player.Stop();
        var toLook = m_interactable.position - m_goToInteract.position;
        toLook = new Vector3(toLook.x, 0, toLook.z).normalized;
        //_player.transform.forward = toLook;
        StartCoroutine(_player.DoAttack(toLook));
    }

    void Update()
    {
        
    }
}
