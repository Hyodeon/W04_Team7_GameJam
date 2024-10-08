using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>플레이어의 정보 저장</para>
/// <para>플레이어 정보를 바탕으로 병아리들 배치</para>
/// <para>Follower 관리</para>
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
        _followCamera.AddFollower();
    }

    public void DeleteObejctFromList(GameObject go)
    {
        _followerList.Remove(go);
        _followCamera.DeleteFollower();
    }

    public void RefreshFollowPoint()
    {
        float radius = Mathf.Sqrt(1.2f * 30 * 0.5f);

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
        if (other.CompareTag("Trap"))
        {
            // 파티클 뿌리기
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Particles/PlayerDestroyedParticle");
            Instantiate(prefab, transform.position, Quaternion.identity);
            Ending.Instance.ShowEnding((int)EEndingList.Death);
            Destroy(gameObject);
        }
    }

}


#region [Legacy] Chick Location Random Generation

//void CalculateRadius()
//{
//    radius = Mathf.Sqrt(minDistance * numberOfPoints * 0.5f);
//    radius *= 1.2f;
//    Debug.Log($"점 {numberOfPoints}에 대한 밀도 {minDistance}의 반지름은 {radius} 입니다!");
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

//    Debug.Log($"실제 생성된 점 개수 : {points.Count}");
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
//        Gizmos.DrawSphere(point, 0.7f); // 점들의 위치를 작은 구로 표시
//    }
//}

#endregion