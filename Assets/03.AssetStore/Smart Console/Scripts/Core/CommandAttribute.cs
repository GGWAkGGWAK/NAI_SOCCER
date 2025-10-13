using System;

namespace SmartConsole
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CommandAttribute : Attribute 
    {
        public readonly string desc;

        public CommandAttribute() { }

        public CommandAttribute(string desc)
        {
            this.desc = desc;
        }
    }
}