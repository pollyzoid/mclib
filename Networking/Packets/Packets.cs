using MCLib.Enums;

namespace MCLib.Networking.Packets
{
    /// <summary>
    /// Sent to both clients and server to test the connection.
    /// Disconnect after 1200 ticks in Minecraft.
    /// <para>Packet ID 0x00</para>
    /// </summary>
    [Packet(ID = Packet.KeepAlive)]
    public class KeepAlive : PacketBase
    {}

    /// <summary>
    /// Sent by client to server.
    /// If server accepts, <see cref="LoginResponse"/> is sent, otherwise <see cref="Disconnect"/>.
    /// <para>Packet ID 0x01</para>
    /// </summary>
    [Packet(ID = Packet.Login, Side = PacketSide.Client)]
    public class LoginRequest : PacketBase
    {
        public int Protocol { get; set; }

        public string Username { get; set; }

        /// <summary>
        /// Password for protected servers, not related to user's password
        /// </summary>
        public string Password { get; set; }

        public long MapSeed { get; set; }
        public byte Dimension { get; set; }
    }

    /// <summary>
    /// Sent by server to client if <see cref="LoginRequest"/> was accepted.
    /// <para>Packet ID 0x01</para>
    /// </summary>
    [Packet(ID = Packet.Login, Side = PacketSide.Server)]
    public class LoginResponse : PacketBase
    {
        public int EntityId { get; set; }

        public string ServerNameMaybe { get; set; }
        public string MotdMaybe { get; set; }

        public long MapSeed { get; set; }
        public byte Dimension { get; set; }
    }

    /// <summary>
    /// Sent by client to server. Used for authentication.
    /// <para>Packet ID 0x02</para>
    /// </summary>
    [Packet(ID = Packet.Handshake, Side = PacketSide.Client)]
    public class Handshake : PacketBase
    {
        public string Username { get; set; }
    }

    /// <summary>
    /// Sent by server to client. Used for authentication.
    /// <para>Packet ID 0x02</para>
    /// </summary>
    [Packet(ID = Packet.Handshake, Side = PacketSide.Server)]
    public class HandshakeResponse : PacketBase
    {
        public string ConnectionHash { get; set; }
    }

    /// <summary>
    /// Message from server to clients or vice-versa.
    /// Maximum length 103 bytes when sent by client.
    /// <para>Packet ID 0x03</para>
    /// </summary>
    [Packet(ID = Packet.ChatMessage)]
    public class ChatMessage : PacketBase
    {
        /// <remarks>
        /// Server kicks if over 100 characters
        /// </remarks>
        public string Message { get; set; }
    }

    /// <summary>
    /// Time of the day
    /// <para>Packet ID 0x04</para>
    /// </summary>
    [Packet(ID = Packet.TimeUpdate)]
    public class TimeUpdate : PacketBase
    {
        /// <summary>
        /// World time in minutes, range 0-24000,
        /// incremented by 20 every second.
        /// </summary>
        public long Time { get; set; }
    }

    /// <summary>
    /// Equipped item and armor of a player
    /// <para>Packet ID 0x05</para>
    /// </summary>
    [Packet(ID = Packet.EntityEquipment)]
    public class EntityEquipment : PacketBase
    {
        public int Entity { get; set; }
        public short Slot { get; set; }
        public short ItemId { get; set; }
    }

    /// <summary>
    /// Sent by server after login to specify spawn location
    /// and to initialize compasses.
    /// <para>Packet ID 0x06</para>
    /// </summary>
    [Packet(ID = Packet.SpawnPosition)]
    public class SpawnPosition : PacketBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    /// <summary>
    /// Sent by client when an entity is used.
    /// <para>Packet ID 0x07</para>
    /// </summary>
    [Packet(ID = Packet.UseEntity)]
    public class UseEntity : PacketBase
    {
        public int User { get; set; }
        public int Target { get; set; }
        public bool LeftClick { get; set; }
    }

    /// <summary>
    /// Sent by server to update a client's health
    /// <para>Packet ID 0x08</para>
    /// </summary>
    [Packet(ID = Packet.UpdateHealth)]
    public class UpdateHealth : PacketBase
    {
        /// <summary>
        /// Ranges from 0 (dead) to 20 (full health)
        /// </summary>
        public short Health { get; set; }
    }

    /// <summary>
    /// Sent by the client when the player presses the "Respawn" button after dying.
    /// The client will not leave the respawn screen until it receives a respawn packet.
    /// <para>Packet ID 0x09</para>
    /// </summary>
    /// <remarks>
    /// After receiving, server drops the player's inventory, teleports the user to the spawn point,
    /// and sends a respawn packet in response.
    /// </remarks>
    [Packet(ID = Packet.Respawn)]
    public class Respawn : PacketBase
    {
    }

    /// <summary>
    /// Sent by the client each tick.
    /// <para>Packet ID 0x0A</para>
    /// </summary>
    [Packet(ID = Packet.PlayerFlying)]
    public class PlayerFlying : PacketBase
    {
        public bool OnGround { get; set; }
    }

    /// <summary>
    /// Updates the players XYZ position on the server.
    /// <para>Packet ID 0x0B</para>
    /// </summary>
    [Packet(ID = Packet.PlayerPosition)]
    public class PlayerPosition : PacketBase
    {
        public double X { get; set; }
        public double FootHeight { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public bool OnGround { get; set; }
    }

    /// <summary>
    /// Updates the direction the player is looking at.
    /// <para>Packet ID 0x0C</para>
    /// </summary>
    /// <remarks>
    /// Yaw is measured in degrees, and does not follow classical trigonometry rules.
    /// The unit circle of yaw on the xz-plane starts at (0, 1) and turns backwards towards (-1, 0),
    /// or in other words, it turns clockwise instead of counterclockwise.
    /// Additionally, yaw is not clamped to between 0 and 360 degrees;
    /// any number is valid, including negative numbers and numbers greater than 360.
    /// </remarks>
    [Packet(ID = Packet.PlayerLook)]
    public class PlayerLook : PacketBase
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get; set; }
    }

    /// <summary>
    /// Combination of <see cref="PlayerPosition"/> and <see cref="PlayerLook"/>
    /// <para>Packet ID 0x0D</para>
    /// </summary>
    [Packet(ID = Packet.PlayerPositionLook, Side = PacketSide.Client)]
    public class PlayerPositionLook : PacketBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double FootHeight { get; set; }
        public double Z { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get; set; }
    }

    /// <summary>
    /// Combination of <see cref="PlayerPosition"/> and <see cref="PlayerLook"/>
    /// Otherwise same as client's version, but FootHeight and Y are switched.
    /// <para>Packet ID 0x0D</para>
    /// </summary>
    [Packet(ID = Packet.PlayerPositionLook, Side = PacketSide.Server)]
    public class PlayerPositionLookServer : PacketBase
    {
        public double X { get; set; }
        public double FootHeight { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get; set; }
    }

    /// <summary>
    /// Sent repeatedly by the client as the player mines a block.
    /// <para>Packet ID 0x0E</para>
    /// </summary>
    [Packet(ID = Packet.PlayerDigging)]
    public class PlayerDigging : PacketBase
    {
        public byte Status { get; set; }

        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }
        public byte Face { get; set; }
    }

    /// <summary>
    /// Sent by the client when a block or an item is placed.
    /// <para>Packet ID 0x0F</para>
    /// </summary>
    [Packet(ID = Packet.PlayerBlockPlace)]
    public class PlayerBlockPlace : PacketBase
    {
        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }

        public byte Direction { get; set; }

        public short ItemId { get; set; }
        public byte ItemAmount { get; set; }
        public byte ItemHealth { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            X = stream.Int32();
            Y = stream.Byte();
            Z = stream.Int32();

            Direction = stream.Byte();

            ItemId = stream.Int16();

            if (ItemId == -1) return;
            ItemAmount = stream.Byte();
            ItemHealth = stream.Byte();
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(X);
            writer.Add(Y);
            writer.Add(Z);
            writer.Add(Direction);
            writer.Add(ItemId);

            if (ItemId == -1) return;
            writer.Add(ItemAmount);
            writer.Add(ItemHealth);
        }
    }

    /// <summary>
    /// Sent by the client when the slot selection is changed.
    /// <para>Packet ID 0x10</para>
    /// </summary>
    [Packet(ID = Packet.HoldingChange)]
    public class HoldingChange : PacketBase
    {
        public short SlotId { get; set; }
    }

    /// <summary>
    /// Sent whenever an entity should change animation.
    /// <para>Packet ID 0x12</para>
    /// </summary>
    [Packet(ID = Packet.Animation)]
    public class Animation : PacketBase
    {
        public int EntityID { get; set; }
        public byte Stage { get; set; }
    }

    /// <summary>
    /// Sent by the server when a player comes into visible range.
    /// <para>Packet ID 0x14</para>
    /// </summary>
    [Packet(ID = Packet.NamedEntitySpawn)]
    public class NamedEntitySpawn : PacketBase
    {
        public int Player { get; set; }

        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte Rotation { get; set; }
        public byte Pitch { get; set; }
        public short CurrentItem { get; set; }
    }

    /// <summary>
    /// Sent by the server when an item on the ground comes into visible range.
    /// Client sends this when an item is dropped from a tile or inventory.
    /// <para>Packet ID 0x15</para>
    /// </summary>
    [Packet(ID = Packet.PickupSpawn)]
    public class PickupSpawn : PacketBase
    {
        public int Entity { get; set; }

        public short Item { get; set; }
        public byte Count { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte Rotation { get; set; }
        public byte Pitch { get; set; }
        public byte Roll { get; set; }
    }

    /// <summary>
    /// Sent by the server when someone picks up an item from ground.
    /// <para>Packet ID 0x16</para>
    /// </summary>
    [Packet(ID = Packet.CollectItem)]
    public class CollectItem : PacketBase
    {
        public int Collected { get; set; }
        public int Collector { get; set; }
    }

    [Packet(ID = Packet.AddObject)]
    public class AddObject : PacketBase
    {
        public int Entity { get; set; }
        public byte Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    [Packet(ID = Packet.MobSpawn)]
    public class MobSpawn : PacketBase
    {
        public int Entity { get; set; }
        public byte Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
    }

    [Packet(ID = Packet.EntityVelocity)]
    public class EntityVelocity : PacketBase
    {
        public int Entity { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
    }

    [Packet(ID = Packet.DestroyEntity)]
    public class DestroyEntity : PacketBase
    {
        public int Entity { get; set; }
    }

    [Packet(ID = Packet.Entity)]
    public class Entity : PacketBase
    {
        public int EID { get; set; }
    }

    [Packet(ID = Packet.EntityRelativeMove)]
    public class EntityRelativeMove : PacketBase
    {
        public int EID { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Z { get; set; }
    }

    [Packet(ID = Packet.EntityLook)]
    public class EntityLook : PacketBase
    {
        public int EID { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
    }

    [Packet(ID = Packet.EntityLookRelativeMove)]
    public class EntityLookRelativeMove : PacketBase
    {
        public int EID { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
    }

    [Packet(ID = Packet.EntityTeleport)]
    public class EntityTeleport : PacketBase
    {
        public int EID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
    }

    [Packet(ID = Packet.EntityStatus)]
    public class EntityStatus : PacketBase
    {
        public int EID { get; set; }
        public byte Status { get; set; }
    }

    [Packet(ID = Packet.AttachEntity)]
    public class AttachEntity : PacketBase
    {
        public int EID { get; set; }
        public int Vehicle { get; set; }
    }

    [Packet(ID = Packet.PreChunk)]
    public class PreChunk : PacketBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Mode { get; set; }
    }

    [Packet(ID = Packet.MapChunk)]
    public class MapChunk : PacketBase
    {
        public int X { get; set; }
        public short Y { get; set; }
        public int Z { get; set; }

        public byte SizeX { get; set; }
        public byte SizeY { get; set; }
        public byte SizeZ { get; set; }

        public byte[] Data { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            X = stream.Int32();
            Y = stream.Int16();
            Z = stream.Int32();

            SizeX = stream.Byte();
            SizeY = stream.Byte();
            SizeZ = stream.Byte();

            Data = stream.Bytes(stream.Int32());
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(X);
            writer.Add(Y);
            writer.Add(Z);
            writer.Add(SizeX);
            writer.Add(SizeY);
            writer.Add(SizeZ);
            writer.Add(Data.Length);
            writer.Add(Data);
        }
    }

    [Packet(ID = Packet.MultiBlockChange)]
    public class MultiBlockChange : PacketBase
    {
        public int X { get; set; }
        public int Z { get; set; }

        public short Size { get; set; }

        public short[] Coordinates { get; set; }
        public byte[] Types { get; set; }
        public byte[] Metadata { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            X = stream.Int32();
            Z = stream.Int32();

            Size = stream.Int16();

            Coordinates = new short[Size];

            for (short i = 0; i < Size; ++i)
            {
                Coordinates[i] = stream.Int16();
            }

            Types = stream.Bytes(Size);
            Metadata = stream.Bytes(Size);
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(X);
            writer.Add(Z);
            writer.Add(Size);
            writer.Add(Coordinates);
            writer.Add(Types);
            writer.Add(Metadata);
        }
    }

    [Packet(ID = Packet.BlockChange)]
    public class BlockChange : PacketBase
    {
        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }

        public byte Type { get; set; }
        public byte Metadata { get; set; }
    }

    [Packet(ID = Packet.Explosion)]
    public class Explosion : PacketBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public float RadiusMaybe { get; set; }

        public int Count { get; set; }
        public byte[][] Records { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            X = stream.Double();
            Y = stream.Double();
            Z = stream.Double();

            RadiusMaybe = stream.Single();

            Count = stream.Int32();

            Records = new byte[Count][];

            for (int i = 0; i < Count; ++i)
            {
                Records[i] = stream.Bytes(3);
            }
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(X);
            writer.Add(Y);
            writer.Add(RadiusMaybe);
            writer.Add(Count);

            for (int i = 0; i < Count; ++i)
            {
                writer.Add(Records[i]);
            }
        }
    }

    [Packet(ID = Packet.InventoryOpen)]
    public class InventoryOpen : PacketBase
    {
        public byte InvId { get; set; }
        public byte InvType { get; set; }
        public string InvName { get; set; }
        public byte InvSlots { get; set; }
    }

    [Packet(ID = Packet.InventoryClose)]
    public class InventoryClose : PacketBase
    {
        public byte InvId { get; set; }
    }

    [Packet(ID = Packet.InventoryClick)]
    public class InventoryClick : PacketBase
    {
        public byte InvId { get; set; }
        public short SlotId { get; set; }
        public bool RightClick { get; set; }
        public short NumClicks { get; set; }
        public short ItemId { get; set; }
        public byte ItemCount { get; set; }
        public byte ItemHealth { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            InvId = stream.Byte();
            SlotId = stream.Int16();
            RightClick = stream.Boolean();
            NumClicks = stream.Int16();
            ItemId = stream.Int16();

            if (ItemId == -1) return;
            ItemCount = stream.Byte();
            ItemHealth = stream.Byte();
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(InvId);
            writer.Add(SlotId);
            writer.Add(RightClick);
            writer.Add(NumClicks);
            writer.Add(ItemId);

            if (ItemId == -1) return;
            writer.Add(ItemCount);
            writer.Add(ItemHealth);
        }
    }

    [Packet(ID = Packet.InventoryUpdate)]
    public class InventoryUpdate : PacketBase
    {
        public byte InvId { get; set; }
        public short SlotId { get; set; }
        public short ItemId { get; set; }
        public byte ItemCount { get; set; }
        public byte ItemHealth { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            InvId = stream.Byte();
            SlotId = stream.Int16();
            ItemId = stream.Int16();

            if (ItemId == -1) return;
            ItemCount = stream.Byte();
            ItemHealth = stream.Byte();
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(InvId);
            writer.Add(SlotId);
            writer.Add(ItemId);

            if (ItemId == -1) return;
            writer.Add(ItemCount);
            writer.Add(ItemHealth);
        }
    }

    [Packet(ID = Packet.InventoryFullUpdate)]
    public class InventoryFullUpdate : PacketBase
    {
        public byte Type { get; set; }
        public short Count { get; set; }
        public Item[] Items { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            Type = stream.Byte();
            Count = stream.Int16();

            Items = new Item[Count];

            for (int i = 0; i < Count; ++i)
            {
                var id = (Enums.Item)stream.Int16();
                if (id == Enums.Item.Invalid)
                    Items[i] = new Item { Id = id, Count = 0, Health = 0 };
                else
                    Items[i] = new Item { Id = id, Count = stream.Byte(), Health = stream.Int16() };
            }
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(Type);
            writer.Add(Count);

            foreach (var item in Items)
            {
                writer.Add((short)item.Id);

                if (item.Id == Enums.Item.Invalid) continue;

                writer.Add(item.Count);
                writer.Add(item.Health);
            }
        }

        public class Item
        {
            public Enums.Item Id { get; set; }
            public byte Count { get; set; }
            public short Health { get; set; }
        }
    }

    [Packet(ID = Packet.UpdateProgressBar)]
    public class Unknown1 : PacketBase
    {
        public byte WindowID { get; set; }
        public short ProgressBar { get; set; }
        public short Value { get; set; }
    }

    [Packet(ID = Packet.Transaction)]
    public class Unknown2 : PacketBase
    {
        public byte WindowID { get; set; }
        public short Action { get; set; }
        public bool Accepted { get; set; }
    }

    [Packet(ID = Packet.SignUpdate)]
    public class SignUpdate : PacketBase
    {
        public int X { get; set; }
        public short Y { get; set; }
        public int Z { get; set; }

        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
    }

    [Packet(ID = Packet.Disconnect)]
    public class Disconnect : PacketBase
    {
        public string Reason { get; set; }
    }
}
