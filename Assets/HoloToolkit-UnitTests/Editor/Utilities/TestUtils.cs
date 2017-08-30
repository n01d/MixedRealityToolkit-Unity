﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoloToolkit.Unity
{
    public static class TestUtils
    {
        /// <summary>
        /// Deletes all objects in the scene
        /// </summary>
        public static void ClearScene()
        {
            ClearUnreferencedActive();
            ClearUnreferencedDisabledInTestScene();
        }

        private static void ClearUnreferencedDisabledInTestScene()
        {
            DestroyGameObjects(SceneManager.GetActiveScene().GetRootGameObjects());
        }

        private static void ClearUnreferencedActive()
        {
            DestroyTransforms(Object.FindObjectsOfType<Transform>());
        }
        
        private static void DestroyTransforms(IEnumerable<Transform> transforms)
        {
            DestroyGameObjects(transforms.Where(t => t).Select(t => t.root.gameObject).Distinct());
        }

        private static void DestroyGameObjects(IEnumerable<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        /// <summary>
        /// Creates a camera and adds the MainCamera tag to it
        /// </summary>
        /// <returns>The created camera</returns>
        public static Camera CreateMainCamera()
        {
            var camera = new GameObject().AddComponent<Camera>();
            camera.gameObject.tag = "MainCamera";
            return camera;
        }

        /// <summary>
        /// Calls Awake on all MonoBehaviours on the given gameObject through reflection.
        /// </summary>
        /// <param name="gameObject">The gameObject to be awoken</param>
        /// <returns>The given gameObject to be able to chain call</returns>
        internal static GameObject CallAwake(this GameObject gameObject)
        {
            foreach (var script in gameObject.GetComponentsInChildren<MonoBehaviour>())
            {
                script.Call("Awake");
            }
            return gameObject;
        }

        /// <summary>
        /// Calls Start on all MonoBehaviours on the given gameObject through reflection.
        /// </summary>
        /// <param name="gameObject">The gameObject to be started</param>
        /// <returns>The given gameObject to be able to chain call</returns>
        internal static GameObject CallStart(this GameObject gameObject)
        {
            foreach (var script in gameObject.GetComponentsInChildren<MonoBehaviour>())
            {
                script.Call("Start");
            }
            return gameObject;
        }

        /// <summary>
        /// Calls Update on all MonoBehaviours on the given gameObject through reflection.
        /// </summary>
        /// <param name="gameObject">The gameObject to be updated</param>
        /// <returns>The given gameObject to be able to chain call</returns>
        internal static GameObject CallUpdate(this GameObject gameObject)
        {
            foreach (var script in gameObject.GetComponentsInChildren<MonoBehaviour>())
            {
                script.Call("Update");
            }
            return gameObject;
        }

        /// <summary>
        /// Call a method with the given name from the given object through reflection
        /// </summary>
        /// <param name="obj">Object to call the method on</param>
        /// <param name="methodName">The method that should be called</param>
        private static void Call(this object obj, string methodName)
        {
            const BindingFlags findFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var methodInfo = obj.GetType().GetMethod(methodName, findFlags);
            if (methodInfo == null) return;
            methodInfo.Invoke(obj, null);
        }
    }
}
