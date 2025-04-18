using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// GPS经纬度和Vector3之间的转换
/// </summary>
public class GisPointTo3DPointController
{


    public Transform Unity_PointA; //Unity中A点  （X正方向和Z轴的负方向之间）
    public Transform Unity_PointB;//Unity中B点  （Z轴正方向和X轴负方向之间）

    private Vector2 AnotherCoordinateSystem_PointA;//另外坐标系中对应的A点位置  
    private Vector2 AnotherCoordinateSystem_PointB;//另外坐标系中对应的B点位置  

    private float z_offset, x_offset, z_w_offset, x_w_offset;
    /// <summary>
    /// 设置另外坐标系中AB两个点的位置
    /// </summary>
    /// <param name="GPS_PointA"></param>
    /// <param name="GPS_PointB"></param>
    public void SetGPS_PointAAndB(Vector2 GPS_PointA, Vector2 GPS_PointB)
    {
        //通过传过来的第一个点，确定坐标范围。以导弹为中心
        //右下
        this.AnotherCoordinateSystem_PointA =new Vector2( GPS_PointA.x, GPS_PointA.y);
        //左上
        this.AnotherCoordinateSystem_PointB = new Vector2(GPS_PointB.x, GPS_PointB.y);
    }
    /// <summary>
    /// 设置Unity中的A点和B点
    /// </summary>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    public void SetUnityPointAAndB(Transform pointA,Transform pointB)
    {
        Unity_PointA = pointA;
        Unity_PointB = pointB;
    }
    public void OnInit()
    {
       
        InitBasicNum();//初始化参数
    }


    private void InitBasicNum()
    {
        z_offset = AnotherCoordinateSystem_PointA.y - AnotherCoordinateSystem_PointB.y;//新坐标系中横轴差  
        x_offset = AnotherCoordinateSystem_PointA.x - AnotherCoordinateSystem_PointB.x;//新坐标系中纵轴差 
        z_w_offset = Unity_PointA.position.z - Unity_PointB.position.z;//unity中的横轴差 
        x_w_offset = Unity_PointA.position.x - Unity_PointB.position.x;//unity中的纵轴差  
    }
    /// <summary>
    /// 由经纬度得到位置点  
    /// </summary>
    /// <param name="se">x经度</param>
    /// y 纬度
    /// <returns></returns>
    public Vector3 GetWorldPoint(float x, float y)
    {
        float tempX = x - AnotherCoordinateSystem_PointB.x;
        float tempZ = y - AnotherCoordinateSystem_PointA.y;
        float _tempX = (tempX * x_w_offset / x_offset + Unity_PointB.position.x);
        float _tempZ = (tempZ * z_w_offset / z_offset + Unity_PointA.position.z);
        //坐标偏差
        return new Vector3(_tempX, 0, _tempZ);
    }
    /// <summary>
    /// 由位置点得到经纬度  
    /// </summary>
    /// <param name="curPoint"></param>
    /// <returns></returns>
    public Vector2 GetLatLon(Vector3 curPoint)
    {
        //坐标偏差
        float _x_offset = (curPoint.x - Unity_PointA.position.x) * x_offset / x_w_offset;
        float _z_offset = (curPoint.z - Unity_PointB.position.z) * z_offset / z_w_offset;
        float resultX = _x_offset + AnotherCoordinateSystem_PointA.x;
        float resultZ = _z_offset + AnotherCoordinateSystem_PointB.y;
        return new Vector2(resultX, resultZ);
    }

}
