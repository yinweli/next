// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Packet1.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace next.client.proto {

  /// <summary>Holder for reflection information generated from Packet1.proto</summary>
  public static partial class Packet1Reflection {

    #region Descriptor
    /// <summary>File descriptor for Packet1.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static Packet1Reflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1QYWNrZXQxLnByb3RvIkUKB1BhY2tldDESEAoIcGxheWVySWQYASABKAUS",
            "DQoFbW9uZXkYAiABKAUSDAoEdGltZRgEIAEoAxILCgN2aXAYAyABKAhCPgoZ",
            "bmV4dC5uZXQubmV0dHkudGVzdC5wcm90b0INUGFja2V0MVdhcHBlcqoCEW5l",
            "eHQuY2xpZW50LnByb3RvYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::next.client.proto.Packet1), global::next.client.proto.Packet1.Parser, new[]{ "PlayerId", "Money", "Time", "Vip" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Packet1 : pb::IMessage<Packet1> {
    private static readonly pb::MessageParser<Packet1> _parser = new pb::MessageParser<Packet1>(() => new Packet1());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Packet1> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::next.client.proto.Packet1Reflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Packet1() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Packet1(Packet1 other) : this() {
      playerId_ = other.playerId_;
      money_ = other.money_;
      time_ = other.time_;
      vip_ = other.vip_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Packet1 Clone() {
      return new Packet1(this);
    }

    /// <summary>Field number for the "playerId" field.</summary>
    public const int PlayerIdFieldNumber = 1;
    private int playerId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int PlayerId {
      get { return playerId_; }
      set {
        playerId_ = value;
      }
    }

    /// <summary>Field number for the "money" field.</summary>
    public const int MoneyFieldNumber = 2;
    private int money_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Money {
      get { return money_; }
      set {
        money_ = value;
      }
    }

    /// <summary>Field number for the "time" field.</summary>
    public const int TimeFieldNumber = 4;
    private long time_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Time {
      get { return time_; }
      set {
        time_ = value;
      }
    }

    /// <summary>Field number for the "vip" field.</summary>
    public const int VipFieldNumber = 3;
    private bool vip_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Vip {
      get { return vip_; }
      set {
        vip_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Packet1);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Packet1 other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PlayerId != other.PlayerId) return false;
      if (Money != other.Money) return false;
      if (Time != other.Time) return false;
      if (Vip != other.Vip) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (PlayerId != 0) hash ^= PlayerId.GetHashCode();
      if (Money != 0) hash ^= Money.GetHashCode();
      if (Time != 0L) hash ^= Time.GetHashCode();
      if (Vip != false) hash ^= Vip.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (PlayerId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(PlayerId);
      }
      if (Money != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Money);
      }
      if (Vip != false) {
        output.WriteRawTag(24);
        output.WriteBool(Vip);
      }
      if (Time != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(Time);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (PlayerId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PlayerId);
      }
      if (Money != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Money);
      }
      if (Time != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Time);
      }
      if (Vip != false) {
        size += 1 + 1;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Packet1 other) {
      if (other == null) {
        return;
      }
      if (other.PlayerId != 0) {
        PlayerId = other.PlayerId;
      }
      if (other.Money != 0) {
        Money = other.Money;
      }
      if (other.Time != 0L) {
        Time = other.Time;
      }
      if (other.Vip != false) {
        Vip = other.Vip;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            PlayerId = input.ReadInt32();
            break;
          }
          case 16: {
            Money = input.ReadInt32();
            break;
          }
          case 24: {
            Vip = input.ReadBool();
            break;
          }
          case 32: {
            Time = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code