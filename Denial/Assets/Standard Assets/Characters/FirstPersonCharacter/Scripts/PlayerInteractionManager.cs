using System;
using UnityEngine;

    [Serializable]
    public class PlayerInteractionManager
    {
        // Know what objects are clickable
        public LayerMask clickableLayer;

        // Swap Cursors per object
        public Texture2D pointer;
        public Texture2D target;
        public Texture2D doorway;

        // Update is called once per frame
        public void UpdateInteraction()
        {
            HandleCursor();
            HandleInteraction();
        }

        void HandleInteraction()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
            {
                if (Input.GetMouseButton(0) && hit.collider.gameObject.tag.Equals(nameof(doorway)))
                {
                    var doorway = hit.collider.gameObject.transform;
                    //TODO open door animation and load new scene
                }
            }
        }

        void HandleCursor()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
            {
                bool door = false;
                if (hit.collider.gameObject.tag.Equals(nameof(doorway)))
                {
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    door = true;
                }
                else
                {
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                }

                if (Input.GetMouseButton(0) && door)
                {
                    var doorway = hit.collider.gameObject.transform;
                    //TODO open door animation and load new scene
                }
            }
            else
            {
                Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
            }
        }
    }
