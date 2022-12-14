//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Scripts/Player/PlayerInputControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""2696a3a4-6e42-4335-9770-8cffa4a60f1e"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""303c53f1-2cdc-4d78-879c-90631ee1832b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse Position"",
                    ""type"": ""Value"",
                    ""id"": ""4b4bb633-08a0-4a8d-b97f-d7d40f499a57"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""30e64700-8184-4106-91a7-7f490058f8e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Select Ground Platform"",
                    ""type"": ""Button"",
                    ""id"": ""5769a897-4771-4466-b6d6-ffd29b2ee757"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Select Air Platform"",
                    ""type"": ""Button"",
                    ""id"": ""0022c77d-f684-4986-b124-4c266957b3de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Spawn"",
                    ""type"": ""Button"",
                    ""id"": ""1ed17b48-a511-4bc3-9c4e-44a193ebf618"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Despawn"",
                    ""type"": ""Button"",
                    ""id"": ""d27f5347-e4dc-4b8d-af2a-f04b9d2fa4f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""31d72206-fa30-4d6b-bd9e-2bec6cae1f76"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""edb53145-cedc-443b-8c3d-0e0e03a867c2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e0d39337-bfe3-4c16-9906-8c21ce2231a7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""954777ed-daf7-43af-8038-283be22e44d6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""985498be-ee9c-484f-a0f6-09b9ed11fcd3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f439c30d-c8c9-49e2-b6da-96b8e3c060d6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8abafc5b-6cfd-4281-86b7-47b8d003677c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab5b997c-bea3-4f77-b83e-1bbdf06fe41d"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select Ground Platform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b886a45-fdd2-4b96-9434-d974828155a6"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select Air Platform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4d4423b-2500-45cf-9d9b-03be53f6684a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Spawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5756d0fc-d74b-4134-b9c1-b8f6a961f61e"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b8eda4e-3702-41de-92a6-253b6acc5866"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Despawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c77dd187-783b-4c2e-aabd-9b89ded079f9"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_MousePosition = m_Player.FindAction("Mouse Position", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_SelectGroundPlatform = m_Player.FindAction("Select Ground Platform", throwIfNotFound: true);
        m_Player_SelectAirPlatform = m_Player.FindAction("Select Air Platform", throwIfNotFound: true);
        m_Player_Spawn = m_Player.FindAction("Spawn", throwIfNotFound: true);
        m_Player_Despawn = m_Player.FindAction("Despawn", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_MousePosition;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_SelectGroundPlatform;
    private readonly InputAction m_Player_SelectAirPlatform;
    private readonly InputAction m_Player_Spawn;
    private readonly InputAction m_Player_Despawn;
    private readonly InputAction m_Player_Interact;
    public struct PlayerActions
    {
        private @PlayerInputControls m_Wrapper;
        public PlayerActions(@PlayerInputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @MousePosition => m_Wrapper.m_Player_MousePosition;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @SelectGroundPlatform => m_Wrapper.m_Player_SelectGroundPlatform;
        public InputAction @SelectAirPlatform => m_Wrapper.m_Player_SelectAirPlatform;
        public InputAction @Spawn => m_Wrapper.m_Player_Spawn;
        public InputAction @Despawn => m_Wrapper.m_Player_Despawn;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @MousePosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @SelectGroundPlatform.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectGroundPlatform;
                @SelectGroundPlatform.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectGroundPlatform;
                @SelectGroundPlatform.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectGroundPlatform;
                @SelectAirPlatform.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectAirPlatform;
                @SelectAirPlatform.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectAirPlatform;
                @SelectAirPlatform.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectAirPlatform;
                @Spawn.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawn;
                @Spawn.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawn;
                @Spawn.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpawn;
                @Despawn.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDespawn;
                @Despawn.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDespawn;
                @Despawn.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDespawn;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @SelectGroundPlatform.started += instance.OnSelectGroundPlatform;
                @SelectGroundPlatform.performed += instance.OnSelectGroundPlatform;
                @SelectGroundPlatform.canceled += instance.OnSelectGroundPlatform;
                @SelectAirPlatform.started += instance.OnSelectAirPlatform;
                @SelectAirPlatform.performed += instance.OnSelectAirPlatform;
                @SelectAirPlatform.canceled += instance.OnSelectAirPlatform;
                @Spawn.started += instance.OnSpawn;
                @Spawn.performed += instance.OnSpawn;
                @Spawn.canceled += instance.OnSpawn;
                @Despawn.started += instance.OnDespawn;
                @Despawn.performed += instance.OnDespawn;
                @Despawn.canceled += instance.OnDespawn;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSelectGroundPlatform(InputAction.CallbackContext context);
        void OnSelectAirPlatform(InputAction.CallbackContext context);
        void OnSpawn(InputAction.CallbackContext context);
        void OnDespawn(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
