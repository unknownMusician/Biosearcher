// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f43be611-ce0e-4bf0-aaa9-12c5f792d077"",
            ""actions"": [
                {
                    ""name"": ""MoveStart"",
                    ""type"": ""Value"",
                    ""id"": ""c8e01934-65fe-44bd-817b-59dd821cdca0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveStop"",
                    ""type"": ""Value"",
                    ""id"": ""7bd750dd-245c-428e-ba1b-1e4c7b907d50"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""75a7f1ce-7bb6-40bb-81d2-eb3edcd4b07f"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveStart"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""13db1af3-0f91-41b7-a664-a8cdb36955ee"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1d05b87b-ebd1-42ac-a103-c855d0cffef9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8a1caabd-35fa-484c-abbe-57845477dac1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""86457c03-1267-4ebf-8403-6491e264905d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""941abf96-e81b-49d4-9c31-e4cbaf97eb6a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""a9a9b03f-2c92-423a-ab94-767ffb23a104"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveStop"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d24ca70f-5113-4317-906c-e67f7dd6ba43"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a296cf9c-21d7-4cb1-a978-92a640160a2e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""130476b0-0cad-4908-a228-ad5a0137df49"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9d0e4f0b-0b17-4602-b8dc-d4cb4650ffc2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""MoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ae7193aa-d99d-4fc4-ac0c-9a18acf4ba0f"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""52c53913-be3e-44ef-afcf-72aca7e5d177"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7e315d8b-de14-4c20-bb82-73c2a83e09bb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveStart"",
                    ""type"": ""Value"",
                    ""id"": ""98479d87-c39c-40f4-966f-7f1b9dc3d1c4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveStop"",
                    ""type"": ""Value"",
                    ""id"": ""146d9db5-88e6-4266-a03b-6d2127f98563"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bcbf0a7f-a190-40fc-85ec-032558e05020"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40d3ba1c-95c7-4f70-b41f-423c4931814d"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8482d2c2-a108-49af-8c78-cf0179db36c8"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78d93ab1-bb81-4aad-8f7f-5206b5bcdccd"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Terraformer"",
            ""id"": ""3d98999e-086f-41a5-96a5-3492344efe9a"",
            ""actions"": [
                {
                    ""name"": ""AddStart"",
                    ""type"": ""Button"",
                    ""id"": ""d80edf32-8e44-4d94-a989-d0572fd49380"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddStop"",
                    ""type"": ""Button"",
                    ""id"": ""6d65461e-4164-40d2-8e5d-88ea4f233011"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveStart"",
                    ""type"": ""Button"",
                    ""id"": ""899306e4-0214-41b2-ae2b-ac1b52646622"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveStop"",
                    ""type"": ""Button"",
                    ""id"": ""3dcba410-13b2-4276-9bca-944d606b34e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""70798630-7616-42fb-90df-4dee8c3407c9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""AddStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45dc5adc-2bb6-4b2a-9636-4e21593ada10"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AddStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""324c4b31-7550-4745-b74a-65f0a33a336b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""AddStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e6668df-c7a4-449b-9f6c-a3f943a4064f"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AddStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""155a6940-d7f5-48a1-961b-0e0f23f5ca85"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""RemoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""951743ef-c553-4c49-978f-5d4bc5a95c3c"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RemoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1196c8ba-75f0-4939-87c5-383049439ac9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""RemoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""750326f6-103a-4481-88ed-c76b548566d3"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RemoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveStart = m_Player.FindAction("MoveStart", throwIfNotFound: true);
        m_Player_MoveStop = m_Player.FindAction("MoveStop", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Move = m_Camera.FindAction("Move", throwIfNotFound: true);
        m_Camera_MoveStart = m_Camera.FindAction("MoveStart", throwIfNotFound: true);
        m_Camera_MoveStop = m_Camera.FindAction("MoveStop", throwIfNotFound: true);
        // Terraformer
        m_Terraformer = asset.FindActionMap("Terraformer", throwIfNotFound: true);
        m_Terraformer_AddStart = m_Terraformer.FindAction("AddStart", throwIfNotFound: true);
        m_Terraformer_AddStop = m_Terraformer.FindAction("AddStop", throwIfNotFound: true);
        m_Terraformer_RemoveStart = m_Terraformer.FindAction("RemoveStart", throwIfNotFound: true);
        m_Terraformer_RemoveStop = m_Terraformer.FindAction("RemoveStop", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MoveStart;
    private readonly InputAction m_Player_MoveStop;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveStart => m_Wrapper.m_Player_MoveStart;
        public InputAction @MoveStop => m_Wrapper.m_Player_MoveStop;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MoveStart.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveStart;
                @MoveStart.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveStart;
                @MoveStart.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveStart;
                @MoveStop.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveStop;
                @MoveStop.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveStop;
                @MoveStop.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveStop;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveStart.started += instance.OnMoveStart;
                @MoveStart.performed += instance.OnMoveStart;
                @MoveStart.canceled += instance.OnMoveStart;
                @MoveStop.started += instance.OnMoveStop;
                @MoveStop.performed += instance.OnMoveStop;
                @MoveStop.canceled += instance.OnMoveStop;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_Move;
    private readonly InputAction m_Camera_MoveStart;
    private readonly InputAction m_Camera_MoveStop;
    public struct CameraActions
    {
        private @Controls m_Wrapper;
        public CameraActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Camera_Move;
        public InputAction @MoveStart => m_Wrapper.m_Camera_MoveStart;
        public InputAction @MoveStop => m_Wrapper.m_Camera_MoveStop;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @MoveStart.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStart;
                @MoveStart.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStart;
                @MoveStart.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStart;
                @MoveStop.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStop;
                @MoveStop.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStop;
                @MoveStop.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStop;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MoveStart.started += instance.OnMoveStart;
                @MoveStart.performed += instance.OnMoveStart;
                @MoveStart.canceled += instance.OnMoveStart;
                @MoveStop.started += instance.OnMoveStop;
                @MoveStop.performed += instance.OnMoveStop;
                @MoveStop.canceled += instance.OnMoveStop;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // Terraformer
    private readonly InputActionMap m_Terraformer;
    private ITerraformerActions m_TerraformerActionsCallbackInterface;
    private readonly InputAction m_Terraformer_AddStart;
    private readonly InputAction m_Terraformer_AddStop;
    private readonly InputAction m_Terraformer_RemoveStart;
    private readonly InputAction m_Terraformer_RemoveStop;
    public struct TerraformerActions
    {
        private @Controls m_Wrapper;
        public TerraformerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @AddStart => m_Wrapper.m_Terraformer_AddStart;
        public InputAction @AddStop => m_Wrapper.m_Terraformer_AddStop;
        public InputAction @RemoveStart => m_Wrapper.m_Terraformer_RemoveStart;
        public InputAction @RemoveStop => m_Wrapper.m_Terraformer_RemoveStop;
        public InputActionMap Get() { return m_Wrapper.m_Terraformer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TerraformerActions set) { return set.Get(); }
        public void SetCallbacks(ITerraformerActions instance)
        {
            if (m_Wrapper.m_TerraformerActionsCallbackInterface != null)
            {
                @AddStart.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStart;
                @AddStart.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStart;
                @AddStart.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStart;
                @AddStop.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStop;
                @AddStop.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStop;
                @AddStop.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStop;
                @RemoveStart.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStart;
                @RemoveStart.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStart;
                @RemoveStart.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStart;
                @RemoveStop.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStop;
                @RemoveStop.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStop;
                @RemoveStop.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStop;
            }
            m_Wrapper.m_TerraformerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AddStart.started += instance.OnAddStart;
                @AddStart.performed += instance.OnAddStart;
                @AddStart.canceled += instance.OnAddStart;
                @AddStop.started += instance.OnAddStop;
                @AddStop.performed += instance.OnAddStop;
                @AddStop.canceled += instance.OnAddStop;
                @RemoveStart.started += instance.OnRemoveStart;
                @RemoveStart.performed += instance.OnRemoveStart;
                @RemoveStart.canceled += instance.OnRemoveStart;
                @RemoveStop.started += instance.OnRemoveStop;
                @RemoveStop.performed += instance.OnRemoveStop;
                @RemoveStop.canceled += instance.OnRemoveStop;
            }
        }
    }
    public TerraformerActions @Terraformer => new TerraformerActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMoveStart(InputAction.CallbackContext context);
        void OnMoveStop(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMoveStart(InputAction.CallbackContext context);
        void OnMoveStop(InputAction.CallbackContext context);
    }
    public interface ITerraformerActions
    {
        void OnAddStart(InputAction.CallbackContext context);
        void OnAddStop(InputAction.CallbackContext context);
        void OnRemoveStart(InputAction.CallbackContext context);
        void OnRemoveStop(InputAction.CallbackContext context);
    }
}
