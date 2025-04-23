using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class SceneActivator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objects;

        private void OnEnable()
        {
            SceneLoader.OnLoadedScene += TurnOnScene;
        }

        private void OnDisable()
        {
            SceneLoader.OnLoadedScene -= TurnOnScene;
        }

        private void TurnOnScene()
        {
            foreach (var _object in objects)
            {
                _object.SetActive(true);
            }
        }
    }
}