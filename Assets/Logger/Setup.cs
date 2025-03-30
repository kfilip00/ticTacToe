using System;
using UnityEngine;

namespace LoggerNS
{
    [Serializable]
    public class Setup
    {
        [field: SerializeField] public Category Category { get; private set; } = Category.Default;
        [field: SerializeField] public Color Color { get; private set; } = Color.white;
        [field: SerializeField] public bool Allowed { get; private set; }
        [field: SerializeField] public string Prefix { get; private set; }

        public void SetAllowed(bool _status)
        {
            Allowed = _status;
        }
    }
}