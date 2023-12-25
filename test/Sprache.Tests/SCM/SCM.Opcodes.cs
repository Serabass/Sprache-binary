using System;

namespace SpracheBinary.Tests.SCM
{
  [AttributeUsage(AttributeTargets.Struct)]
  public class OpcodeAttribute : System.Attribute
  {
    public OpcodeAttribute(SCMOpcode opcode)
    {
      this.opcode = opcode;
    }

    public SCMOpcode opcode;
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
