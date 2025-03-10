using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace NavKeypad
{
    public class KeypadButton : MonoBehaviour
    {
        [Header("Value")]
        [SerializeField] private string value;
        [Header("Button Animation Settings")]
        [SerializeField] private float bttnspeed = 0.1f;
        [SerializeField] private float moveDist = 0.0025f;
        [SerializeField] private float buttonPressedTime = 0.1f;
        [Header("Component References")]
        [SerializeField] private Keypad keypad;

        private XRBaseInteractable interactable;

        //void Awake()
        //{
        //    interactable = GetComponent<XRBaseInteractable>();
        //    interactable.hoverEntered.AddListener(PressButton);
        //}

        public void PressButton()
        {
            if (!moving)
            {
                keypad.AddInput(value);
                StartCoroutine(MoveSmooth());
            }
        }

        void Awake()
		{
			// Get the XRBaseInteractable component attached to this GameObject
			interactable = GetComponent<XRBaseInteractable>();

			// Check if the component exists
			if (interactable != null)
			{
				// Add a listener for the hover enter event
				interactable.hoverEntered.AddListener(OnHoverEnter);
                Debug.Log("added listener");
			}
			else
			{
				Debug.LogError("XRBaseInteractable component not found." + gameObject.name);
			}
		}

		// Define the method to be called when the hover enter event is triggered
		void OnHoverEnter(HoverEnterEventArgs args)
		{
			Debug.Log("Hover enter event triggered!");
            PressButton();
		}
		private bool moving;

        private IEnumerator MoveSmooth()
        {

            moving = true;
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = transform.localPosition + new Vector3(0, 0, moveDist);

            float elapsedTime = 0;
            while (elapsedTime < bttnspeed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / bttnspeed);

                transform.localPosition = Vector3.Lerp(startPos, endPos, t);

                yield return null;
            }
            transform.localPosition = endPos;
            yield return new WaitForSeconds(buttonPressedTime);
            startPos = transform.localPosition;
            endPos = transform.localPosition - new Vector3(0, 0, moveDist);

            elapsedTime = 0;
            while (elapsedTime < bttnspeed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / bttnspeed);

                transform.localPosition = Vector3.Lerp(startPos, endPos, t);

                yield return null;
            }
            transform.localPosition = endPos;

            moving = false;
        }

        void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject.tag == "hammer")
            //{
            //    PressButton();
            //}
			PressButton();
		}
    }
}