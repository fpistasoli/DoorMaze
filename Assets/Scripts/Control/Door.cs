using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Manager;

namespace Maze.Control
{

    public class Door : MonoBehaviour
    {

        public enum doorType { Green, Blue, Locked };

        public doorType type;

        public bool isOpen = false;

        public bool isOpenInitially = false;

     // private enum doorPosition { Vertical, Horizontal };

     // private doorPosition position;

        public Sprite[] spriteArray;

        //VERSION ACTUAL:
        // [gvc, bvc, lvc]

        // VERSION ANTERIOR:
        // g: green, b: blue, l: locked, v: vertical, h: horizontal, c: closed, o: open
        // [gvc, gvo, ghc, gho, bvc, bvo, bhc, bho, lvc, lvo, lhc, lho]

        public static Door sharedInstance;

        void Awake()
        {

            sharedInstance = this;

            if (type == doorType.Blue || type == doorType.Locked)
            {

                isOpenInitially = false;

                isOpen = false;

            }
            else
            {

                isOpenInitially = isOpen;

            }

            //SetPosition();

        }

        void Update()
        {

            UpdateSpriteType(); // actualizo la imagen de acuerdo a si la puerta esta abierta o no
            HideColliderIfOpen(); // si esta abierta, saco el collider

        }

        void FixedUpdate()
        {

            //activo el isTrigger para las puertas rojas (las que tienen la cerradura), ya que al tocarlas se abren si tenemos la llave roja
            if ((type == doorType.Locked) && (GameManager.sharedInstance.collectedKeys > 0))
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
            }

        }

        void UpdateSpriteType()
        {
            // renderizo la imagen correspondiente al objeto Door a instanciar
            Sprite sprite = null; // sprite inicial a renderizar

            switch (type)
            {
                case doorType.Green:
                    sprite = spriteArray[0];
                    break;
                
                case doorType.Blue:
                    sprite = spriteArray[1];
                    break;

                case doorType.Locked:
                    sprite = spriteArray[2];
                    break;
            }

            GetComponent<SpriteRenderer>().sprite = sprite;

            UpdateSpriteIfOpen(sprite);
            
            //GetComponent<Renderer>().enabled = true;

        }

        private void UpdateSpriteIfOpen(Sprite sprite)
        {
            Color spriteColor = GetComponent<SpriteRenderer>().color;
            if (isOpen)
            {
                spriteColor.a = 0.3f; //make sprite almost transparent
            } else {
                spriteColor.a = 1f; //make sprite fully opaque
            }
            GetComponent<SpriteRenderer>().color = spriteColor;

            //dividir la puerta en 4 partes: la 2a y 3a volverlas transparentes (usar alpha color para
            //volver estas partes transparentes)

        }


        // VERSION ANTERIOR DE UPDATESPRITE:
        /*
                void UpdateSprite()
                {

                    // renderizo la imagen correspondiente al objeto Door a instanciar
                    Sprite sprite = null; // sprite inicial a renderizar

                    switch (type)
                    {

                        case doorType.Green:
                            if (position == doorPosition.Vertical)
                            {
                                if (!isOpen)
                                {
                                    sprite = spriteArray[0];
                                }
                                else
                                {
                                    sprite = spriteArray[1];
                                }
                                break;
                            }
                            else
                            { // horizontal
                                if (!isOpen)
                                {
                                    sprite = spriteArray[2];
                                }
                                else
                                {
                                    sprite = spriteArray[3];
                                }
                                break;
                            }

                        case doorType.Blue:
                            if (position == doorPosition.Vertical)
                            {
                                if (!isOpen)
                                {
                                    sprite = spriteArray[4];
                                }
                                else
                                {
                                    sprite = spriteArray[5];
                                }
                                break;
                            }
                            else
                            { // horizontal
                                if (!isOpen)
                                {
                                    sprite = spriteArray[6];
                                }
                                else
                                {
                                    sprite = spriteArray[7];
                                }
                                break;
                            }

                        case doorType.Locked:
                            if (position == doorPosition.Vertical)
                            {
                                if (!isOpen)
                                {
                                    sprite = spriteArray[8];
                                    print(spriteArray[8].name);

                                }
                                else
                                {
                                    sprite = spriteArray[9];
                                }
                                break;
                            }
                            else
                            { // horizontal
                                if (!isOpen)
                                {
                                    sprite = spriteArray[10];
                                }
                                else
                                {
                                    sprite = spriteArray[11];
                                }
                                break;
                            }
                    }

                    //GetComponent<Renderer>().enabled = true;

                }
        */


        void HideColliderIfOpen()
        {
            // elimino el collider si la puerta está abierta
            if (isOpen)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }

        }


        void OnTriggerEnter2D(Collider2D otherCollider)
        {

            int collectedKeys = GameManager.sharedInstance.collectedKeys;

            // se abre solo si el jugador tiene al menos una llave roja y la puerta es roja
            if ((otherCollider.tag == "Player") && (type == doorType.Locked) && collectedKeys > 0)
            {

                isOpen = true;
                GameManager.sharedInstance.collectedKeys--;
            }


            //Debug.Log("Cantidad de llaves: " + GameManager.sharedInstance.collectedKeys);


        }



        public void RestoreInitialProperties()
        {

            //SetPosition();

            isOpen = isOpenInitially;

            //vuelvo a bloquear el paso por las puertas rojas (ya que inicialmente estan cerradas)
            if (type == doorType.Locked)
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
            }

        }




  /*      public void SetPosition()
        {
            float positionZ = this.transform.rotation.eulerAngles.z;
            print(positionZ);

            //evalúo si está en posición horizontal o vertical
            if (positionZ % 180 == 0)
            {
                print(this.gameObject + " está en posicion horizontal");
                position = doorPosition.Horizontal; //esta en posición horizontal
            }
            else
            { //suponiendo que el usuario colocó la puerta únicamente o en posición horizontal o vertical
                print(this.gameObject + " está en posicion vertical");
                position = doorPosition.Vertical; //esta en posición vertical
            }
        }

    }
*/

    }

}