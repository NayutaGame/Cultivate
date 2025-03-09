
using System;
using UnityEngine;

[Serializable]
public class RoomReport : TestReport
{
    [SerializeReference]
    public RoomEntry RoomEntry;

    [SerializeReference]
    public RoomDescriptor RoomDescriptor;

    private RoomReport(
        RoomEntry roomEntry,
        RoomDescriptor roomDescriptor
    )
    {
        RoomEntry = roomEntry;
        RoomDescriptor = roomDescriptor;
    }

    public static RoomReport FromEnvironment(RunEnvironment env, Room room)
    {
        return new(
            roomEntry:                  room.GetEntry(),
            roomDescriptor:             room.GetDescriptor()
        );
    }
}
