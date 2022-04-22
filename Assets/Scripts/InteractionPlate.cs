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
    float m_originalInteractableHeight;
    [SerializeField] DropSlotsManager m_DropSlotter;
    public DropSlotsManager DropSlotter => m_DropSlotter;

    public enum INTERACTION_TYPE
    {
        CHOPPER,
        TREE,
        DROPPER
    }
    [SerializeField] INTERACTION_TYPE m_interactionType = INTERACTION_TYPE.CHOPPER;
    public INTERACTION_TYPE InteractionType => m_interactionType;

    private void Start()
    {
        m_originalInteractableHeight = m_interactable.localPosition.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Activable)
        {
            var p = other.GetComponent<PersonController>();
            if (!p.CanInteract(this))
                return;
            Activable = false;
            m_plate.DOLocalMoveY(ActivatedHeight, .2f);
            StartCoroutine(WaitAndDoInteraction(p));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Activable)
        {
            m_plate.DOLocalMoveY(m_originalInteractableHeight, .2f).OnComplete(() => Activable = true);
        }
    }

    IEnumerator WaitAndDoInteraction(PersonController _person)
    {
        //yield return new WaitForSeconds(.1f);
        _person.State = PersonController.STATE.INTERACTING;
        NavMesh.SamplePosition(m_goToInteract.position, out NavMeshHit hit, .1f, NavMesh.AllAreas);
        var playerT = _person.transform;
        //var playerA = _player.Agent;
        while ((m_goToInteract.position - playerT.position).magnitude > .05f) //new Vector2(playerT.position.x, playerT.position.z) != new Vector2(m_goToInteract.position.x, m_goToInteract.position.z))
        {
            _person.DoMove(m_goToInteract.position - playerT.position);
            yield return new WaitForEndOfFrame();
        }
        playerT.position = hit.position;
        _person.Stop();
        var toLook = m_interactable.position - m_goToInteract.position;
        toLook = new Vector3(toLook.x, 0, toLook.z).normalized;
        //_player.transform.forward = toLook;
        _person.DoInteraction(this, toLook);
    }
}
