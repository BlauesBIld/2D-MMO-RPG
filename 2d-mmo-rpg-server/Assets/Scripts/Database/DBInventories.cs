using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBInventories
{
    public static bool SaveInventoryToDB(InventoryObj invObj)
    {
        DAOGame dao = DAOGame.GetInstance();

        return dao.UpdateInventoryForUserChar(invObj);
    }

    public static string GetInventoryFromUserChar(InventoryObj invObj)
    {
        DAOGame dao = DAOGame.GetInstance();

        return dao.SelectInventoryFromUserChar(invObj);
    }
}
