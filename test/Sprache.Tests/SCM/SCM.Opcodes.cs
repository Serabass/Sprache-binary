using System;

namespace Sprache.Binary.Tests.SCM
{
  [AttributeUsage(AttributeTargets.Struct)]
  public class OpcodeAttribute(SCMOpcode opcode) : Attribute
  {
    public SCMOpcode opcode = opcode;
  }

  [Obsolete("This is a test struct, not a real SCM struct. It is only used to test the parser.")]
  [Opcode(SCMOpcode.NAME_THREAD)]
  public struct NameThread
  {
    public string name;
  }

  public enum SCMOpcode : ushort
  {
    NAME_THREAD = 0x03A4,
  }
}
