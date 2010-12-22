using MCLib.Enums;

namespace MCLib.Networking.Packets
{
    [Packet(Id = Packet.KeepAlive)]
    public class KeepAlive : PacketBase
    {}

    [Packet(Id = Packet.Login, Side = PacketSide.Client)]
    public class LoginRequest : PacketBase
    {
        /// <summary>
        /// Network protocol version, latest is 7
        /// </summary>
        public int Protocol { get; set; }

        public string Username { get; set; }

        /// <summary>
        /// Password for protected servers, not related to user's password
        /// </summary>
        public string Password { get; set; }

        public long Seed { get; set; }
        public byte Dimension { get; set; }
    }

    [Packet(Id = Packet.Login, Side = PacketSide.Server)]
    public class LoginResponse : PacketBase
    {
        /// <summary>
        /// Player's entity
        /// </summary>
        public int EntityId { get; set; }

        public string ServerNameMaybe { get; set; }
        public string MotdMaybe { get; set; }

        public long MapSeed { get; set; }
        public byte Dimension { get; set; }
    }

    [Packet(Id = Packet.Handshake, Side = PacketSide.Client)]
    public class Handshake : PacketBase
    {
        public string Username { get; set; }
    }

    [Packet(Id = Packet.Handshake, Side = PacketSide.Server)]
    public class HandshakeResponse : PacketBase
    {
        public string ConnectionHash { get; set; }
    }

    [Packet(Id = Packet.ChatMessage)]
    public class ChatMessage : PacketBase
    {
        public string Message { get; set; }
    }

    [Packet(Id = Packet.TimeUpdate)]
    public class TimeUpdate : PacketBase
    {
        public long Time { get; set; }
    }

    [Packet(Id = Packet.PlayerInventory)]
    public class PlayerInventory : PacketBase
    {
        public int Type { get; set; }
        public short Count { get; set; }
        public Item[] Items { get; set; }

        public override void Read(NetworkStreamMC stream)
        {
            Type = stream.Int32();
            Count = stream.Int16();

            Items = new Item[Count];

            for (int i = 0; i < Count; ++i)
            {
                var id = (Enums.Item)stream.Int16();
                if (id == Enums.Item.Invalid)
                    Items[i] = new Item {Id = id, Count = 0, Health = 0};
                else
                    Items[i] = new Item {Id = id, Count = stream.Byte(), Health = stream.Int16()};
            }
        }

        protected override void Write(PacketWriter writer)
        {
            writer.Add(Type);
            writer.Add(Count);

            foreach(var item in Items)
            {
                writer.Add((short) item.Id);

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

    [Packet(Id = Packet.SpawnPosition)]
    public class SpawnPosition : PacketBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    [Packet(Id = Packet.UseEntity)]
    public class UseEntity : PacketBase
    {
        public int User { get; set; }
        public int Target { get; set; }
        public bool LeftClick { get; set; }
    }

    [Packet(Id = Packet.UpdateHealth)]
    public class UpdateHealth : PacketBase
    {
        public short Health { get; set; }
    }

    [Packet(Id = Packet.Respawn)]
    public class Respawn : PacketBase
    {
    }

    [Packet(Id = Packet.PlayerFlying)]
    public class PlayerFlying : PacketBase
    {
        public bool OnGround { get; set; }
    }

    [Packet(Id = Packet.PlayerPosition)]
    public class PlayerPosition : PacketBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Stance { get; set; }
        public double Z { get; set; }

        public bool OnGround { get; set; }
    }

    [Packet(Id = Packet.PlayerLook)]
    public class PlayerLook : PacketBase
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get; set; }
    }

    [Packet(Id = Packet.PlayerPositionLook, Side = PacketSide.Client)]
    public class PlayerPositionLook : PacketBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Stance { get; set; }
        public double Z { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get; set; }
    }

    // Goddamnit Notch
    [Packet(Id = Packet.PlayerPositionLook, Side = PacketSide.Server)]
    public class PlayerPositionLookServer : PacketBase
    {
        public double X { get; set; }
        public double Stance { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get; set; }
    }

    [Packet(Id = Packet.PlayerDigging)]
    public class PlayerDigging : PacketBase
    {
        public byte Status { get; set; }

        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }
        public byte Face { get; set; }
    }

    [Packet(Id = Packet.PlayerBlockPlace)]
    public class PlayerBlockPlace : PacketBase
    {
        public short ItemId { get; set; }
        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }

        public byte Direction { get; set; }
    }

    [Packet(Id = Packet.HoldingChange)]
    public class HoldingChange : PacketBase
    {
        public short ItemId { get; set; }
    }

    [Packet(Id = Packet.Animation)]
    public class Animation : PacketBase
    {
        public int Player { get; set; }
        public byte Stage { get; set; }
    }

    [Packet(Id = Packet.NamedEntitySpawn)]
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

    [Packet(Id = Packet.PickupSpawn)]
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

    [Packet(Id = Packet.CollectItem)]
    public class CollectItem : PacketBase
    {
        public int Collected { get; set; }
        public int Collector { get; set; }
    }

    [Packet(Id = Packet.AddObject)]
    public class AddObject : PacketBase
    {
        public int Entity { get; set; }
        public byte Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    [Packet(Id = Packet.MobSpawn)]
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

    [Packet(Id = Packet.EntityVelocity)]
    public class EntityVelocity : PacketBase
    {
        public int Entity { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
    }

    [Packet(Id = Packet.DestroyEntity)]
    public class DestroyEntity : PacketBase
    {
        public int Entity { get; set; }
    }

    [Packet(Id = Packet.Entity)]
    public class Entity : PacketBase
    {
        public int EID { get; set; }
    }

    [Packet(Id = Packet.EntityRelativeMove)]
    public class EntityRelativeMove : PacketBase
    {
        public int EID { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Z { get; set; }
    }

    [Packet(Id = Packet.EntityLook)]
    public class EntityLook : PacketBase
    {
        public int EID { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
    }

    [Packet(Id = Packet.EntityLookRelativeMove)]
    public class EntityLookRelativeMove : PacketBase
    {
        public int EID { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
    }

    [Packet(Id = Packet.EntityTeleport)]
    public class EntityTeleport : PacketBase
    {
        public int EID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
    }

    [Packet(Id = Packet.EntityStatus)]
    public class EntityStatus : PacketBase
    {
        public int EID { get; set; }
        public byte Status { get; set; }
    }

    [Packet(Id = Packet.AttachEntity)]
    public class AttachEntity : PacketBase
    {
        public int EID { get; set; }
        public int Vehicle { get; set; }
    }

    [Packet(Id = Packet.PreChunk)]
    public class PreChunk : PacketBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Mode { get; set; }
    }

    [Packet(Id = Packet.MapChunk)]
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

    [Packet(Id = Packet.MultiBlockChange)]
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

    [Packet(Id = Packet.BlockChange)]
    public class BlockChange : PacketBase
    {
        public int X { get; set; }
        public byte Y { get; set; }
        public int Z { get; set; }

        public byte Type { get; set; }
        public byte Metadata { get; set; }
    }

    [Packet(Id = Packet.Explosion)]
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

    [Packet(Id = Packet.InventoryOpen)]
    public class InventoryOpen : PacketBase
    {
        public byte InvId { get; set; }
        public byte InvType { get; set; }
        public string InvName { get; set; }
        public byte InvSlots { get; set; }
    }

    [Packet(Id = Packet.InventoryClose)]
    public class InventoryClose : PacketBase
    {
        public byte InvId { get; set; }
    }

    [Packet(Id = Packet.InventoryClick)]
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

    [Packet(Id = Packet.InventoryUpdate)]
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

    [Packet(Id = Packet.InventoryFullUpdate)]
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

    [Packet(Id = Packet.Unknown1)]
    public class Unknown1 : PacketBase
    {
        public byte Dunno1 { get; set; }
        public short Dunno2 { get; set; }
        public short Dunno3 { get; set; }
    }

    [Packet(Id = Packet.Unknown2)]
    public class Unknown2 : PacketBase
    {
        public byte Dunno1 { get; set; }
        public short Dunno2 { get; set; }
        public bool Dunno3 { get; set; }
    }

    [Packet(Id = Packet.SignUpdate)]
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

    [Packet(Id = Packet.Disconnect)]
    public class Disconnect : PacketBase
    {
        public string Reason;
    }
}
