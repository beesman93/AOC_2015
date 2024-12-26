using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AOC_2015
{
    internal class Day07: BaseDayWithInput
    {
        enum OP
        {
            AND,
            OR,
            LSHIFT,
            RSHIFT,
            NOT,
            ASSIGN,
            LITERAL
        }

        class Component
        {
            private ushort? _value;
            public OP Op { get; set; }
            public Component? Input1 { get; set; }
            public Component? Input2 { get; set; }
            public Component(string id, OP op = OP.LITERAL)
            {
                this.Op = op;
                if (ushort.TryParse(id, out var val))
                    Value = val;
            }
            public void ResetValue() => _value = null;
            public ushort Value
            {
                get
                {
                    #pragma warning disable CS8602 // That's the point, it autofills when needed from nested components
                    if (!_value.HasValue) _value = Op switch
                    {
                        OP.AND => (ushort)(Input1.Value & Input2.Value),
                        OP.OR => (ushort)(Input1.Value | Input2.Value),
                        OP.LSHIFT => (ushort)(Input1.Value << Input2.Value),
                        OP.RSHIFT => (ushort)(Input1.Value >> Input2.Value),
                        OP.NOT => (ushort)(~Input2.Value),
                        OP.ASSIGN => Input2.Value,
                        OP.LITERAL => throw new Exception($"Value of {Op} operation has to be set."),
                        _ => throw new NotImplementedException("OP not implemented")
                    };
                    #pragma warning restore CS8602
                    return _value.Value;
                }
                set
                {
                    _value = value;
                }
            }
        }

        private static Component GetOrAdd(string id, ref Dictionary<string, Component> components)
        {
            if (!components.ContainsKey(id)) components[id] = new Component(id);
            return components[id];
        }

        readonly Dictionary<string,Component> components;

        public Day07()
        {
            components = [];
            foreach (var line in _input)
            {
                var lr = line.Split(" -> ");
                var l = lr[0].Split(" ");
                Component? input1 = l.Length==3 ? GetOrAdd(l[0], ref components) : null;
                Component? input2 = GetOrAdd(l.Last(), ref components);
                Component? output = GetOrAdd(lr[1], ref components);
                output.Input1 = input1;
                output.Input2 = input2;
                output.Op = l.Length > 1 ? l[^2] switch
                {
                    "AND" => OP.AND,
                    "OR" => OP.OR,
                    "LSHIFT" => OP.LSHIFT,
                    "RSHIFT" => OP.RSHIFT,
                    "NOT" => OP.NOT,
                    _ => throw new NotImplementedException($"OP not implemented")
                } : OP.ASSIGN;
            }
        }
        public override ValueTask<string> Solve_1()
        {
            return new($"{components["a"].Value}");
        }
        public override ValueTask<string> Solve_2()
        {
            var newBliteral = components["a"].Value;
            foreach (var comp in components.Values.Where(x => x.Op!=OP.LITERAL))
                comp.ResetValue();
            components["b"].Value = newBliteral;
            return new($"{components["a"].Value}");
        }
    }
}
