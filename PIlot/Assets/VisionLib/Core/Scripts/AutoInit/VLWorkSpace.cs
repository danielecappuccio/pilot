/** @addtogroup vlUnitySDK
 *  @{
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///  The VLWorkSpace allows the definition of an area, from which the user can initialize the
///  tracking.
///  **THIS IS SUBJECT TO CHANGE** Please do not rely on this code in productive environments.
/// </summary>
[AddComponentMenu("VisionLib/AutoInit/VL WorkSpace")]
public class VLWorkSpace : VLWorkerReferenceBehaviour
{
    [HideInInspector]
    public VLGeometry sourceGeometry;

    [HideInInspector]
    public GameObject destinationGeometry;

    [HideInInspector]
    public Vector3 upVector = Vector3.up;

    [HideInInspector]
    public bool displayViewDirection = true;

    [HideInInspector]
    public bool previewWithCamera = false;

    [HideInInspector]
    public Camera usedCamera;

    [HideInInspector]
    public int camSliderPosition;

    [HideInInspector]
    [System.NonSerialized]
    public VIS.WorkSpace.Transform[] poses;

    private const float defaultRotationRange = 20.0f;
    private const float defaultRotationStep = 20.0f;

    private static VIS.WorkSpace.Geometry CreateVLGeometry(VLGeometry creator,
        VLUnityCameraHelper.FlipCoordinateSystemHandedness fcsHandedness)
    {
        return creator.CreateVLGeometry(VLUnityCameraHelper.CreateLocalVLTransform(
            creator.gameObject,
            fcsHandedness));
    }

    private VIS.WorkSpace.Geometry GetSourceGeometry(
        VLUnityCameraHelper.FlipCoordinateSystemHandedness fcsHandedness)
    {
        return CreateVLGeometry(this.sourceGeometry, fcsHandedness);
    }

    private VIS.WorkSpace.Geometry GetDestinationGeometry(
        VLUnityCameraHelper.FlipCoordinateSystemHandedness fcsHandedness)
    {
        VLGeometry creator = this.destinationGeometry.GetComponent<VLGeometry>();

        // check if object has a geometry component
        if (creator != null)
        {
            return CreateVLGeometry(creator, fcsHandedness);
        }
        else
        {
            // if target object has no geometry component -> use center point which is stored in
            // destinationPoints[0]
            VIS.WorkSpace.Transform trans = VLUnityCameraHelper.CreateLocalVLTransform(
                this.destinationGeometry,
                fcsHandedness);
            Vector3[] destination = GetDestinationVertices();
            trans.t = new float[3] { destination[0].x, destination[0].y, destination[0].z };

            return new VIS.WorkSpace.Plane(
                0,
                0,
                1,
                trans);
        }
    }

    /// <summary>
    /// Gets the positions represented by the DestinationGeometry.
    /// If the destinationGeometry is a Renderer, it will calculate the center of the model.
    /// </summary>
    /// <returns>Array of points that represent the object geometry</returns>
    public Vector3[] GetDestinationVertices()
    {
        // check if object has a geometry component
        VLGeometry vlDestinationGeometry = this.destinationGeometry.GetComponent<VLGeometry>();

        if (vlDestinationGeometry != null)
        {
            if (vlDestinationGeometry.currentMesh == null)
            {
                vlDestinationGeometry.UpdateMesh();
            }

            return vlDestinationGeometry.currentMesh;
        }

        // check children for renderer
        List<Renderer> renderers =
            this.destinationGeometry.transform.GetComponentsInChildren<Renderer>().ToList();

        // check target object for renderer
        if (this.destinationGeometry.GetComponent<Renderer>())
        {
            renderers.Add(this.destinationGeometry.GetComponent<Renderer>());
        }

        if (renderers.Count > 0)
        {
            return new[] { GetCenterOfRenderer(renderers) };
        }
        return new[] { this.destinationGeometry.transform.position };
    }

    /// <summary>
    /// Calculate center of a list of renderers.
    /// </summary>
    /// <param name="renderer"></param>
    /// <returns>Center of given renderers</returns>
    private static Vector3 GetCenterOfRenderer(List<Renderer> renderer)
    {
        Bounds bounds = renderer[0].bounds;

        foreach (Renderer rend in renderer)
        {
            bounds.Encapsulate(rend.bounds);
        }

        return bounds.center;
    }

    /// <summary>
    /// Creates a VLWorkSpace.Definition from this VLWorkSpace.
    /// </summary>
    /// <returns>VLWorkSpace.Definition described by this class</returns>
    public VIS.WorkSpace.Definition GetWorkSpaceDefinition(bool useCameraRotation)
    {
        VLUnityCameraHelper.FlipCoordinateSystemHandedness fcsHandedness =
        VLUnityCameraHelper.FlipCoordinateSystemHandedness.Automatically;
        if (InitWorkerReference())
        {
            fcsHandedness = this.workerBehaviour.flipCoordinateSystemHandedness;
        }
        
        VIS.WorkSpace.Definition currentWorkSpaceDef =
            new VIS.WorkSpace.Definition(
                new VIS.WorkSpace.Transform(
                    this.transform.position,
                    this.transform.rotation),
                VLUnityCameraHelper.UnityVectorToVLVector(this.upVector, fcsHandedness),
                useCameraRotation ? defaultRotationRange : 0.0f,
                defaultRotationStep);

        currentWorkSpaceDef.origin = GetSourceGeometry(fcsHandedness);
        currentWorkSpaceDef.destination = GetDestinationGeometry(fcsHandedness);

        return currentWorkSpaceDef;
    }
}

/**@}*/
