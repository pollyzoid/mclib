﻿namespace MCLib.Enums
{
    public enum Packet : byte
    {
        // C-s = Client-to-server only

        KeepAlive = 0x00,
        Login = 0x01,
        Handshake = 0x02,
        ChatMessage = 0x03,
        TimeUpdate = 0x04,
        EntityEquipment = 0x05,
        SpawnPosition = 0x06,
        UseEntity = 0x07,           //C-s
        UpdateHealth = 0x08,
        Respawn = 0x09,
        PlayerFlying = 0x0A,        //C-s
        PlayerPosition = 0x0B,      //C-s
        PlayerLook = 0x0C,          //C-s
        PlayerPositionLook = 0x0D,
        PlayerDigging = 0x0E,       //C-s
        PlayerBlockPlace = 0x0F,    //C-s
        HoldingChange = 0x10,
        //AddToInventory = 0x11,
        Animation = 0x12,
        NamedEntitySpawn = 0x14,
        PickupSpawn = 0x15,
        CollectItem = 0x16,
        AddObject = 0x17,
        MobSpawn = 0x18,
        EntityVelocity = 0x1C,
        DestroyEntity = 0x1D,
        Entity = 0x1E,
        EntityRelativeMove = 0x1F,
        EntityLook = 0x20,
        EntityLookRelativeMove = 0x21,
        EntityTeleport = 0x22,
        EntityStatus = 0x26,
        AttachEntity = 0x27,
        PreChunk = 0x32,
        MapChunk = 0x33,
        MultiBlockChange = 0x34,
        BlockChange = 0x35,
        //ComplexEntities = 0x3B,
        Explosion = 0x3C,

        //New packets, probably changing
        InventoryOpen = 0x64,
        InventoryClose = 0x65,
        InventoryClick = 0x66,
        InventoryUpdate = 0x67,
        InventoryFullUpdate = 0x68,
        UpdateProgressBar = 0x69,
        Transaction = 0x6A,
        SignUpdate = 0x82,

        Disconnect = 0xFF
    }

    public enum PacketSide
    {
        /// <summary>
        /// Packet has same protocol on both server and client
        /// </summary>
        Shared,
        /// <summary>
        /// Packet is client-to-server only
        /// </summary>
        Client,
        /// <summary>
        /// Packet is server-to-client only
        /// </summary>
        Server
    }
}
