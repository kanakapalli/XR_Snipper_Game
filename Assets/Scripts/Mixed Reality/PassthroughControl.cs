using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta;
using Meta.XR.MRUtilityKit;
using Meta.WitAi;

public class PassthroughControl : MonoBehaviour
{
    private MRUKRoom m_MRUK_Room;
    private LayerMask LayerMask;

    public void RemoveKeyWall()
    {
     /*   m_MRUK_Room = MRUK.Instance.GetCurrentRoom();
        Vector2 m_Vec = Vector2.zero;
        MRUKAnchor m_MRUK_Anchor = m_MRUK_Room.GetKeyWall(out m_Vec);
        m_MRUK_Anchor.transform.GetChild(0).GetComponent<MeshRenderer>().DestroySafely();*/
    }
}
