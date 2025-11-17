
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RoomReport : TestReport
{
    [SerializeReference] public LegacyRoomEntry RoomEntry;

    [SerializeReference] public RoomDefinition RoomDefinition;

    private RoomReport(
        LegacyRoomEntry roomEntry,
        RoomDefinition roomDefinition
    )
    {
        RoomEntry = roomEntry;
        RoomDefinition = roomDefinition;
    }

    public static RoomReport FromEnvironment(RunEnvironment env, Room room)
    {
        return new(
            roomEntry:                  room.GetEntry(),
            roomDefinition:             room.GetDescriptor()
        );
    }
}
