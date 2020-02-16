using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimCursor : MonoBehaviour
{
    #region Members
    public static float gs_COOLDOWN = 0.5f;
    
    private float m_DeltaTime;
    private Text m_Text;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        m_Text = gameObject.GetComponent<Text>();
        if (m_Text == null)
        {
            Debug.LogError("Component Text missing ! ");
            return;
        }

        m_DeltaTime = gs_COOLDOWN;
        m_Text.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        m_DeltaTime -= Time.deltaTime;
        if(m_DeltaTime <= .0f)
        {
            m_DeltaTime += gs_COOLDOWN;
            m_Text.enabled = !m_Text.enabled;
        }
    }
}
