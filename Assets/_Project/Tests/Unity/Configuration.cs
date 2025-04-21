using Configuration;
using UnityEngine;

namespace Test
{
    public class Configuration : MonoBehaviour
    {
        public Config GetConfig()
        {
            return new Config
            {
                Database = DatabaseType.Firebase,
                IsTesting = false
            };
        }
    }
}