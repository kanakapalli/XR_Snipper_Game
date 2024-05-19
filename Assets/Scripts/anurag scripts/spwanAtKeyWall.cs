using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwanAtKeyWall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] internal GameObject m_Welcome;
    [SerializeField] internal GameObject m_GoodJob;
    [SerializeField] internal GameObject m_Important;
    [SerializeField] internal PlayerData m_PlayerData;
    [SerializeField] internal GameObject contactor;
    void Start()
    {
        spwanAtKeywall();
        int newUser = PlayerPrefs.GetInt("NewUser", 0);
    }

    // Update is called once per frame


    public void spwanAtKeywall()
    {
        MRUK mruk = MRUK.Instance;
        Vector2 vector2 = Vector2.zero;
        MRUKAnchor keywall = mruk.GetCurrentRoom().GetKeyWall(out vector2);



        this.transform.position = keywall.GetAnchorCenter();

        this.transform.rotation = keywall.gameObject.transform.rotation * Quaternion.Euler(0, 180, 0);

        int _scene_index = PlayerPrefs.GetInt("Scene", 0);

        if (_scene_index == 0)
        {
            spwanContactor(m_Welcome);
            m_PlayerData.m_NewPlayerStatus = NewPlayerStatus.Welcome;
        }
        else if (_scene_index == 1)
        {
            if (m_PlayerData.m_NewPlayerStatus == NewPlayerStatus.Welcome)
            {
                spwanContactor(m_GoodJob);
                m_PlayerData.m_NewPlayerStatus = NewPlayerStatus.GoodJob;
            }
        }
    }


    public void spwanContactor(GameObject _prefab)
    {

        MRUK mruk = MRUK.Instance;
        Vector2 vector2 = Vector2.zero;
        MRUKAnchor keywall = mruk.GetCurrentRoom().GetKeyWall(out vector2);
        MRUKAnchor floor = mruk.GetCurrentRoom().GetFloorAnchor();

        GameObject contactorInstance = Instantiate(_prefab, keywall.gameObject.transform);

        contactorInstance.transform.position = new Vector3(contactorInstance.transform.position.x, floor.gameObject.transform.position.y, contactorInstance.transform.position.z);


        // inistaicate contactor  at keywall.transform

    }
}
