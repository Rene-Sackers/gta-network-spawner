using System;
using System.Collections.Generic;
using GTANetworkServer;
using GTANetworkShared;

public class Spawner : Script
{
    private Dictionary<Client, NetHandle> _vehicleHistory = new Dictionary<Client, NetHandle>();
    
    public Spawner()
    {
        API.onClientEventTrigger += onClientEventTrigger;
    }

    public void onClientEventTrigger(Client sender, string name, object[] args)
    {
        if (name != "CREATE_VEHICLE") return;

        var model = (int)args[0];

        if (!Enum.IsDefined(typeof(VehicleHash), model)) return;

        var rot = API.getEntityRotation(sender.handle);
        var veh = API.createVehicle((VehicleHash)model, sender.position, new Vector3(0, 0, rot.Z), 0, 0);

        if (_vehicleHistory.ContainsKey(sender) && _vehicleHistory[sender] != null && API.doesEntityExist(_vehicleHistory[sender]))
        {
            API.deleteEntity(_vehicleHistory[sender]);
        }

        _vehicleHistory[sender] = veh;

        API.setPlayerIntoVehicle(sender, veh, -1);     
    }
}
