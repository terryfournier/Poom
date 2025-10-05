using System;
using System.Collections;
using UnityEngine;

public class LinksHandleLerp : LinksHandle
{
    [SerializeField] private Transform m_climbTarget;
    [SerializeField] private AnimationClip m_climbClip;
    [SerializeField] private AnimationClip m_preparedClip;
    [SerializeField] private AnimationClip m_jumpClip;
    [SerializeField] private AnimationCurve m_jumpCurve;
    private Animator m_animator;
    private bool m_isAnimator;

    private new void Start()
    {
        base.Start();

        m_animator = GetComponent<Animator>();

        m_linkHere = Lerper;

        m_isAnimator = (m_animator != null);
    }

    private void Lerper()
    {
        if (m_linkType == LinkType.Double)
        {
            if (IsLinkUp())
            {
                if (IsLinkLowEnought())
                {
                    StartCoroutine(ClimbingLink());
                }
                else
                {
                    StartCoroutine(SuperClimbing());
                }
            }
            else
            {
                StartCoroutine(JumpingLink());
            }
        }
        else
        {
            if (m_linkType == LinkType.Fall)
            {
                StartCoroutine(JumpingLink());
            }
            else
            {
                if (IsLinkLowEnought())
                {
                    StartCoroutine(ClimbingLink());
                }
                else
                {
                    StartCoroutine(SuperClimbing());
                }
            }
        }
    }

    private bool IsLinkLowEnought()
    {
        Vector3 endPos = new Vector3(m_StartLink.x, m_EndLink.y, m_StartLink.z);
        if (Vector3.Distance(m_StartLink, endPos) < m_agent.height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsLinkUp()
    {
        return m_agent.transform.position.y < m_EndLink.y;
    }

    private IEnumerator JumpingLink()
    {
        float jumpHeight = Vector3.Distance(m_StartLink, m_EndLink) * 3.0f;
        float duration = m_jumpClip.length;
        float elapsedTime = 0.0f;
        float timer = 0.0f;

        m_animator.SetTrigger("Jump");

        yield return new WaitForSeconds(m_preparedClip.length);

        while (elapsedTime < 1.0f)
        {
            timer += Time.deltaTime;
            elapsedTime = Mathf.Clamp01(timer / duration);
            float heightOffset = m_jumpCurve.Evaluate(elapsedTime) * jumpHeight;

            Vector3 horizontalPos = Vector3.Lerp(m_StartLink, m_EndLink, elapsedTime);
            m_agent.transform.position = horizontalPos + Vector3.up * heightOffset;

            yield return null;
        }
        LinkHere();
    }

    private IEnumerator ClimbingLink()
    {
        Vector3 endlink = m_EndLink;
        endlink.y = m_StartLink.y;
        float distance = Vector3.Distance(m_StartLink, endlink);

        float duration = distance / m_agent.speed;
        float t = 0;
        bool isPlaceCorrect = false;
        while (!isPlaceCorrect)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, m_agent.radius * 3.0f))
            {
                isPlaceCorrect = true;
                yield return null;
            }

            t += Time.deltaTime / duration;
            m_agent.transform.position = Vector3.Lerp(m_StartLink, endlink, t);
            yield return null;
        }

        m_animator.SetTrigger("Climb");
    }

    private IEnumerator SuperClimbing()
    {
        Vector3 endlink = m_EndLink;
        endlink.y = m_StartLink.y;
        float distance = Vector3.Distance(m_StartLink, endlink);
        
        float duration = distance / m_agent.speed;
        float t = 0;
        bool isPlaceCorrect = false;
        while (!isPlaceCorrect)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, m_agent.radius))
            {
                isPlaceCorrect = true;
                yield return null;
            }

            t += Time.deltaTime / duration;
            m_agent.transform.position = Vector3.Lerp(m_StartLink, endlink, t);
            yield return null;
        }

        Vector3 endPos = new Vector3(m_agent.transform.position.x, 
            m_EndLink.y - m_agent.height, 
            m_agent.transform.position.z);

        distance = Vector3.Distance(m_agent.transform.position, endPos);

        duration = m_jumpClip.length;
        float elapsedTime = 0.0f;
        
        m_animator.SetTrigger("Jump");
        yield return new WaitForSeconds(m_preparedClip.length);
        while (elapsedTime < duration)
        {
            float speed = elapsedTime / duration;

            m_agent.transform.position = Vector3.Lerp(m_agent.transform.position, endPos, speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        m_animator.SetTrigger("Climb");
    }

    public void UpdatePos()
    {
        StartCoroutine(FinalLerp());
    }

    private IEnumerator FinalLerp()
    {
        Vector3 pos = m_climbTarget.position;
        transform.position = pos;
        float distance = Vector3.Distance(pos, m_EndLink);
        float duration = distance / m_agent.speed;
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime / duration;
            m_agent.transform.position = Vector3.Lerp(pos, m_EndLink, elapsedTime);

            yield return null;
        }
        LinkHere();
    }
}
