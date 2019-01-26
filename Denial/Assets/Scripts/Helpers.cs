using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helpers
{
    public static class Input
    {
        public static IEnumerable<KeyCode> GetAnyKeys(params KeyCode[] codes)
        {
            foreach (var code in codes)
            {
                if (UnityEngine.Input.GetKey(code))
                    yield return code;
            }
        }
    }
}
