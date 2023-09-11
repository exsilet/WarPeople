using System.Collections;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private GameObject[] _peoplePrefab;

    private GameObject _peopleInstance;
    private bool _firstTake = true;

    private void Awake()
    {
        _firstTake = true;
    }
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => photonView.Owner.GetPlayerNumber() >= 0);
        
        SetupPeopleOnWar(photonView.Owner.GetPlayerNumber());
    }

    private void SetupPeopleOnWar(int gridStartIndex)
    {
        _peopleInstance = Instantiate(_peoplePrefab[gridStartIndex], transform.position, Quaternion.identity);
        
        if (!photonView.IsMine)
        {
            _peopleInstance.SetActive(false);
        }
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            _firstTake = false;
        }
        
        _peopleInstance.transform.SetParent(transform);
    }
    
    private void OnDestroy()
    {
        Destroy(_peopleInstance);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}