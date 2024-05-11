using UnityEngine;

public class CharacterTableTest : MonoBehaviour
{
    public CharacterTable characterTable;
    void Start()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(characterTable.Get(20101));
        }
    }
}
