using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.XR;
using UnityEngine.Events;

namespace BCCH_VR_Therapy
{
    public class SepInputListener_LEFT : MonoBehaviour
    {
        /*
         * [Level of crucial]:3
         *      0= not important, but don't know if there are any portantial risk if you remove this
         *      1= slightly important, but not the part of its function
         *      2= may slightly affect part of side function if you change anything
         *      3= may affect some of the major function if you change anything
         *      4= may break the entire project if you change anything
         * 
         */


        /// <summary>
        /// ACTION BASE
        /// </summary>

        [SerializeField] private InputActionAsset actionAsset;
        [SerializeField] private string controllerName;
        [SerializeField] private string actionNamePrimaryButton;
        [SerializeField] private string actionNameSecondaryButton;
        [SerializeField] private string actionNameMenuButton;

        private InputActionMap _actionMap;
        private InputAction _inputActionPrimaryButton;
        private InputAction _inputActionSecondaryButton;
        private InputAction _inputActionMenuButton;

        /// <summary>
        /// ACTION BASE EXPERIEMENT
        /// </summary>


        //define
        //public XRNode controllerNode;
        // here we are going to define all the situation when we press or release all the button on left and right controller
        // 
        //                                                       ::RISK::
        //                                            VIVE HAVE NO SECONDARY BUTTON
        //                                            VIVE HAVE NO SECONDARY BUTTON
        //                                            VIVE HAVE NO SECONDARY BUTTON

        //LEFT PRIMARY

        [Tooltip("Event when the left primary button starts being pressed")]
        public UnityEvent OnPress_PrimLeft;

        [Tooltip("Event when the left primary button starts being released")]
        public UnityEvent OnRelease_PrimLeft;

        ////keep track of wether we are pressing left primary button
        bool isPressed_PrimLeft = false;

        //LEFT SECONDARY

        [Tooltip("Event when the left secondary button starts being pressed")]
        public UnityEvent OnPress_SeconLeft;

        [Tooltip("Event when the left secondary button starts being released")]
        public UnityEvent OnRelease_SeconLeft;

        //keep track of wether we are pressing left primary button
        bool isPressed_SeconLeft = false;


        //LEFT MENU
        [Tooltip("Event when the left menu button starts being pressed")]
        public UnityEvent OnPress_MenuLeft;

        [Tooltip("Event when the left menu button starts being released")]
        public UnityEvent OnRelease_MenuLeft;

        //keep track of wether we are pressing left menu button
        bool isPressed_MenuLeft = false;

        private void Awake()
        {

            //get all of our actions...
            _actionMap = actionAsset.FindActionMap(controllerName);
            _inputActionPrimaryButton = _actionMap.FindAction(actionNamePrimaryButton);
            _inputActionSecondaryButton = _actionMap.FindAction(actionNameSecondaryButton);
            _inputActionMenuButton = _actionMap.FindAction(actionNameMenuButton);

            //==============
            //LEGACY
            //===============

            //inputDevices_Right = new List<InputDevice>();
            //inputDevices_Left = new List<InputDevice>();
        }

        private void OnEnable()
        {
            _inputActionPrimaryButton.Enable();
            _inputActionSecondaryButton.Enable();
            _inputActionMenuButton.Enable();
        }
        private void OnDisable()
        {
            _inputActionPrimaryButton.Disable();
            _inputActionSecondaryButton.Disable();
            _inputActionMenuButton.Disable();
        }




        // Start is called before the first frame update
        void Start()
        {
            //GetDevices();
        }

        // Update is called once per frame
        void Update()
        {
            var primaryBtnValue = _inputActionPrimaryButton.ReadValue<float>();
            var secondaryBtnValue = _inputActionSecondaryButton.ReadValue<float>();
            var menuBtnValue = _inputActionMenuButton.ReadValue<float>();

            if (menuBtnValue != 0)
            {
                Debug.Log("You pressed the Left Controller menu button");
                if (!isPressed_MenuLeft)
                {
                    isPressed_MenuLeft = true;
                    Debug.Log("OnPress left menu event is called");
                    OnPress_MenuLeft.Invoke();
                }
            }
            else if (isPressed_MenuLeft)
            {
                isPressed_MenuLeft = false;
                OnRelease_MenuLeft.Invoke();
                Debug.Log("OnRelease left menu event is called");
            }

            if (primaryBtnValue != 0)
            {
                Debug.Log("You pressed the Left Controller Primary button");
                if (!isPressed_PrimLeft)
                {
                    isPressed_PrimLeft = true;
                    Debug.Log("OnPress left prim event is called");

                    OnPress_PrimLeft.Invoke();
                }
            }
            else if (isPressed_PrimLeft)
            {
                isPressed_PrimLeft = false;
                OnRelease_PrimLeft.Invoke();
                Debug.Log("OnRelease left prim event is called");
            }

            if (secondaryBtnValue != 0)
            {
                if (!isPressed_SeconLeft)
                {
                    isPressed_SeconLeft = true;
                    Debug.Log("OnPress left secon event is called");

                    OnPress_SeconLeft.Invoke();
                }
            }
            else if (isPressed_SeconLeft)
            {
                isPressed_SeconLeft = false;
                OnRelease_SeconLeft.Invoke();
                Debug.Log("OnRelease left secon event is called");
            }
        }

        //====================
        //LEGACY Perameters
        //=====================

        //List<InputDevice> inputDevices_Right;
        //List<InputDevice> inputDevices_Left;
        //
        //NOTE:
        //there are two kinds of way to access input, device characteristics and XR Node. 
        //Both two ways have their own trait and benefit, yet in our case, they are all valid.
        //Thus, I'll put all of them into this code for leraning and comparing.
        //
        //define device characteristics

        //InputDeviceCharacteristics deviceCharacteristics_Left;



        //====================
        //LEGACY FUNCTION
        //====================

        //void GetDevices()
        //{
        //    //get device through device characteristics
        //    deviceCharacteristics_Left = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left;
        //    InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics_Left, inputDevices_Left);

        //    //get device through XR Node
        //    InputDevices.GetDevicesAtXRNode(controllerNode, inputDevices_Right);
        //}

        //void DeviceBaseUpdate()
        //{
        //    foreach (InputDevice inputDevice in inputDevices_Right)
        //    {
        //        //Debug.Log("Device Found With Name: " + inputDevice.name);
        //        bool rightPrimaryCheck;
        //        if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out rightPrimaryCheck) && rightPrimaryCheck)
        //        {
        //            Debug.Log("You pressed the Right Controller Primary button");
        //            if (!isPressed_PrimRight)
        //            {
        //                isPressed_PrimRight = true;
        //                Debug.Log("OnPress right prim event is called");

        //                OnPress_PrimRight.Invoke();
        //            }
        //        }
        //        else if (isPressed_PrimRight)
        //        {
        //            isPressed_PrimRight = false;
        //            OnRelease_PrimRight.Invoke();
        //            Debug.Log("OnRelease right prim event is called");
        //        }

        //        bool rightSecondaryCheck;
        //        if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out rightSecondaryCheck) && rightSecondaryCheck)
        //        {
        //            if (!isPressed_SeconRight)
        //            {
        //                isPressed_SeconRight = true;
        //                Debug.Log("OnPress right secon event is called");

        //                OnPress_SeconRight.Invoke();
        //            }
        //        }
        //        else if (isPressed_SeconRight)
        //        {
        //            isPressed_SeconRight = false;
        //            OnRelease_SeconRight.Invoke();
        //            Debug.Log("OnRelease right secon event is called");
        //        }
        //    }

        //    foreach (InputDevice inputDevice in inputDevices_Left)
        //    {
        //        //Debug.Log("Device Found With Name: " + inputDevice.name);
        //        bool leftPrimaryCheck;
        //        if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out leftPrimaryCheck) && leftPrimaryCheck)
        //        {
        //            Debug.Log("You pressed the Left Controller Primary button");
        //            if (!isPressed_PrimLeft)
        //            {
        //                isPressed_PrimLeft = true;
        //                Debug.Log("OnPress left prim event is called");

        //                OnPress_PrimLeft.Invoke();
        //            }
        //        }
        //        else if (isPressed_PrimLeft)
        //        {
        //            isPressed_PrimLeft = false;
        //            OnRelease_PrimLeft.Invoke();
        //            Debug.Log("OnRelease left prim event is called");
        //        }

        //        bool leftSecondaryCheck;
        //        if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out leftSecondaryCheck) && leftSecondaryCheck)
        //        {
        //            if (!isPressed_SeconLeft)
        //            {
        //                isPressed_SeconLeft = true;
        //                Debug.Log("OnPress left secon event is called");

        //                OnPress_SeconLeft.Invoke();
        //            }
        //        }
        //        else if (isPressed_SeconLeft)
        //        {
        //            isPressed_SeconLeft = false;
        //            OnRelease_SeconLeft.Invoke();
        //            Debug.Log("OnRelease left secon event is called");
        //        }
        //    }
        //}


    }
}

