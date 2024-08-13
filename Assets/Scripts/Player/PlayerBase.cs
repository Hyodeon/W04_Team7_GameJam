using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>�÷��̾��� ���� ����</para>
/// <para>�÷��̾� ������ �������� ���Ƹ��� ��ġ</para>
/// <para>Follower ����</para>
/// </summary>
public class PlayerBase : MonoBehaviour
{
    private HashSet<GameObject> _followerList;

    private Vector3 _followPoint;

    public FollowCamera _followCamera;

    public int ChickCount { get { return _followerList.Count; } }

    public Vector3 FollowPoint { get { return _followPoint; } }

    private void Awake()
    {
        _followerList = new HashSet<GameObject>();

    }

    private void Update()
    {
        RefreshFollowPoint();
    }

    public void AddFollower(GameObject go)
    {
        _followerList.Add(go);
        if (_followerList.Contains(go))
        {

        }
        else
        {
            _followCamera.AddFollower();
        }
    }

    public void DeleteObejctFromList(GameObject go)
    {
        if (_followerList.Contains(go))
        {
            _followerList.Remove(go);
            _followCamera.DeleteFollower();
        }
    }

    public void RefreshFollowPoint()
    {
        float radius = Mathf.Sqrt(1.2f * 10 * 0.5f);

        _followPoint = transform.position + (-transform.forward) * radius;
    }

    public void PropagateJump()
    {
        foreach (GameObject go in _followerList)
        {
            Vector3 distVector = FollowPoint - go.transform.position;
            distVector.y = 0;
            float distance = (transform.position - go.transform.position).magnitude;

            StartCoroutine(DoJump(go, distance));
        }
    }

    private IEnumerator DoJump(GameObject follower, float distance)
    {
        if (distance < 50)
        {
            Follower foComp = follower.GetComponent<Follower>();

            yield return new WaitForSeconds(distance / foComp.MoveSpeed);
            if (follower.gameObject != null)
            {
                foComp.TriggerState(Follower.State.Jump);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spear"))
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Particles/PlayerDestroyedParticle");
            Instantiate(prefab, transform.position, Quaternion.identity);
            Ending.Instance.ShowEnding((int)EEndingList.Skewers);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Pot"))
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Particles/PlayerDestroyedParticle");
            Instantiate(prefab, transform.position, Quaternion.identity);
            Ending.Instance.ShowEnding((int)EEndingList.Pot);
            Destroy(gameObject);
        }
    }

}


#region [Legacy] Chick Location Random Generation

//void CalculateRadius()
//{
//    radius = Mathf.Sqrt(minDistance * numberOfPoints * 0.5f);
//    radius *= 1.2f;
//    Debug.Log($"�� {numberOfPoints}�� ���� �е� {minDistance}�� �������� {radius} �Դϴ�!");
//}

//void SpawnPointsInCircle(int count)
//{
//    int attempts = 0;

//    while (points.Count < count && attempts < count * 10)
//    {
//        Vector2 point = GetRandomPointInCircle(radius);

//        if (IsPointValid(point))
//        {
//            points.Add(point);
//            //Instantiate(pointPrefab, point, Quaternion.identity);
//        }

//        attempts++;
//    }

//    Debug.Log($"���� ������ �� ���� : {points.Count}");
//}


//Vector2 GetRandomPointInCircle(float radius)
//{
//    float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
//    float r = radius * Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f));
//    float x = r * Mathf.Cos(angle);
//    float y = r * Mathf.Sin(angle);
//    return new Vector2(x, y);
//}
//bool IsPointValid(Vector2 point)
//{
//    foreach (Vector2 existingPoint in points)
//    {
//        if (Vector2.Distance(point, existingPoint) < minDistance)
//        {
//            return false;
//        }
//    }
//    return true;
//}

//void OnDrawGizmos()
//{
//    if (points == null)
//        return;

//    Gizmos.color = Color.red;
//    foreach (Vector2 point in points)
//    {
//        Gizmos.DrawSphere(point, 0.7f); // ������ ��ġ�� ���� ���� ǥ��
//    }
//}

#endregion