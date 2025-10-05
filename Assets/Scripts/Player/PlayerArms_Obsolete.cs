using UnityEngine;

public class PlayerArms_Obsolete : MonoBehaviour
{
    private enum PoseName
    {
        IDLE,
        KNIFE,
        GUN,
        RIFLE,

        POSE_NB
    }

    private const int SHOULDER = 0;
    private const int ELBOW = 1;
    private const int WRIST = 2;
    private const int THUMB = 15;
    private const int INDEX = 3;
    private const int MIDDLE = 6;
    private const int RING = 12;
    private const int PINKY = 9;

    [SerializeField] private Transform[] armLeft = null;
    [SerializeField] private Transform[] armRight = null;
    [SerializeField] PoseName poseName = PoseName.IDLE;

    private Transform weaponAnchor = null;

    // Update is called once per frame
    private void Update()
    {
        SetPose();
    }


    private void SetPose()
    {
        weaponAnchor = GameObject.Find("Anchor").transform;

        switch (poseName)
        {
            case PoseName.IDLE:
                {
                    armLeft[SHOULDER].localPosition = new Vector3(0f, 0f, 0f);
                    armRight[SHOULDER].localPosition = new Vector3(0f, 0f, 0f);
                }
                break;
            case PoseName.KNIFE:
                {
                    weaponAnchor.localPosition = new Vector3(0.0659999996f, 1.20159996f, 0.0452000014f);

                    #region Left arm
                    armLeft[SHOULDER].position = new Vector3(0f, 0f, 0f);
                    #endregion

                    #region Right arm
                    // Shoulder
                    armRight[SHOULDER].localPosition = new Vector3(0.248899996f, 1.30299997f, -0.44600001f);
                    armRight[SHOULDER].localRotation = Quaternion.Euler(new Vector3(86.1187973f, 60.6423149f, 151.512344f));

                    // Elbow
                    armRight[ELBOW].localRotation = Quaternion.Euler(new Vector3(-9.26477787e-06f, 0.0848741755f, 28.9762821f));

                    // Wrist
                    armRight[WRIST].localRotation = Quaternion.Euler(new Vector3(0.0732767433f, 358.082581f, 8.99477386f));

                    // Thumb
                    // Base joint
                    armRight[THUMB].localRotation = Quaternion.Euler(new Vector3(81.7193756f, 112.132057f, 218.06308f));

                    // Middle joint
                    armRight[THUMB + 1].localRotation = Quaternion.Euler(new Vector3(1.49488153e-06f, 5.18868956e-06f, 4.72611427f));

                    // Tip joint
                    armRight[THUMB + 2].localRotation = Quaternion.Euler(new Vector3(3.2702053e-06f, -1.73216063e-06f, 2.51723766f));


                    // Index
                    // Base joint
                    armRight[INDEX].localRotation = Quaternion.Euler(new Vector3(0.304008573f, 179.654129f, 247.22879f));

                    // Middle joint
                    armRight[INDEX + 1].localRotation = Quaternion.Euler(new Vector3(-1.63468103e-05f, -9.02317879e-06f, 335.87854f));

                    // Tip joint
                    armRight[INDEX + 2].localRotation = Quaternion.Euler(new Vector3(56.9996109f, 303.105621f, 327.753113f));


                    // Middle finger
                    // Base joint
                    armRight[MIDDLE].localRotation = Quaternion.Euler(new Vector3(2.73507833f, 184.940216f, 251.409668f));

                    // Middle joint
                    armRight[MIDDLE + 1].localRotation = Quaternion.Euler(new Vector3(1.49061202e-06f, 6.66896494e-06f, 331.221466f));

                    // Tip joint
                    armRight[MIDDLE + 2].localRotation = Quaternion.Euler(new Vector3(1.01803232e-06f, -3.41216651e-06f, 348.389923f));


                    // Ring finger
                    // Base joint
                    armRight[RING].localRotation = Quaternion.Euler(new Vector3(0.304025084f, 179.65416f, 259.424225f));

                    // Middle joint
                    armRight[RING + 1].localRotation = Quaternion.Euler(new Vector3(5.8869332e-06f, -1.04425162e-05f, 331.394073f));

                    // Tip joint
                    armRight[RING + 2].localRotation = Quaternion.Euler(new Vector3(1.17771542e-05f, 8.16814372e-06f, 313.449005f));


                    // Pinky
                    // Base joint
                    armRight[PINKY].localRotation = Quaternion.Euler(new Vector3(0.672211885f, 89.4790497f, 276.62561f));

                    // Middle joint
                    armRight[PINKY + 1].localRotation = Quaternion.Euler(new Vector3(-9.05180332e-06f, 330.764832f, 5.89011643e-06f));

                    // Tip joint
                    armRight[PINKY + 2].localRotation = Quaternion.Euler(new Vector3(7.73612646e-06f, 328.49408f, 2.28703993e-06f));
                    #endregion
                }
                break;
            case PoseName.GUN:
                {
                    weaponAnchor.localPosition = new Vector3(0.196999997f, 1.15600002f, 0.275000006f);

                    #region Left arm
                    armLeft[SHOULDER].localPosition = new Vector3(0f, 0f, 0f);
                    #endregion

                    #region Right arm
                    // Shoulder
                    armRight[SHOULDER].localPosition = new Vector3(0.268400013f, 1.31799996f, -0.342000067f);
                    armRight[SHOULDER].localRotation = Quaternion.Euler(new Vector3(85.3624039f, 131.90123f, 208.623352f));

                    // Elbow
                    armRight[ELBOW].localRotation = Quaternion.Euler(new Vector3(-5.67125835e-06f, 0.0848721862f, 20.5064087f));

                    // Wrist
                    armRight[WRIST].localRotation = Quaternion.Euler(new Vector3(0.0732705072f, 358.082581f, 8.47615433f));

                    // Thumb
                    armRight[THUMB].localRotation = Quaternion.Euler(new Vector3(78.9948959f, 229.371658f, 33.960331f));


                    // Index
                    // Base joint
                    armRight[INDEX].localRotation = Quaternion.Euler(new Vector3(0.304029495f, 179.654144f, 340.196014f));

                    // Middle joint
                    armRight[INDEX + 1].localRotation = Quaternion.Euler(new Vector3(-5.38037557e-06f, -3.33075377e-06f, 346.91687f));

                    // Tip joint
                    armRight[INDEX + 2].localRotation = Quaternion.Euler(new Vector3(0.304029495f, 179.654144f, 340.196014f));


                    // Middle finger
                    // Base joint
                    armRight[MIDDLE].localRotation = Quaternion.Euler(new Vector3(2.7350781f, 184.940216f, 335.381836f));

                    // Middle joint
                    armRight[MIDDLE + 1].localRotation = Quaternion.Euler(new Vector3(-5.03844467e-06f, 4.05315495e-06f, 321.670105f));

                    // Tip joint
                    armRight[MIDDLE + 2].localRotation = Quaternion.Euler(new Vector3(1.01803221e-06f, -3.41216651e-06f, 348.389923f));


                    // Ring finger
                    // Base joint
                    armRight[RING].localRotation = Quaternion.Euler(new Vector3(0.304028571f, 179.65416f, 323.916718f));

                    // Tip joint
                    armRight[RING + 2].localRotation = Quaternion.Euler(new Vector3(6.06202366e-06f, 2.86225873e-06f, 325.549957f));
                    #endregion
                }
                break;
            case PoseName.RIFLE:
                {
                    weaponAnchor.localPosition = new Vector3(0.0810000002f, 1.06400001f, 0.40200001f);

                    #region Left arm
                    // Shoulder
                    armLeft[SHOULDER].localPosition = new Vector3(0.156000122f, 1.20299959f, 0.0739995837f);
                    armLeft[SHOULDER].localRotation = Quaternion.Euler(new Vector3(35.9330025f, 134.400009f, 14.7650166f));

                    // Elbow
                    armLeft[ELBOW].localRotation = Quaternion.Euler(new Vector3(8.89728832f, 351.185181f, 338.895477f));

                    // Wrist
                    armLeft[WRIST].localRotation = Quaternion.Euler(new Vector3(321.479126f, 7.52279711f, 350.190857f));

                    // Thumb
                    armLeft[THUMB].localRotation = Quaternion.Euler(new Vector3(298.68811f, 100.074493f, 28.0396805f));

                    // Index
                    armLeft[INDEX].localRotation = Quaternion.Euler(new Vector3(359.695984f, 0.345849663f, 182.083603f));

                    // Middle finger
                    armLeft[MIDDLE].localRotation = Quaternion.Euler(new Vector3(359.695984f, 0.345849663f, 182.083511f));

                    // Ring finger
                    armLeft[RING].localRotation = Quaternion.Euler(new Vector3(359.695984f, 0.345849663f, 182.083603f));
                    #endregion

                    #region Right arm
                    // Shoulder
                    armRight[SHOULDER].localPosition = new Vector3(0.201000005f, 1.28999901f, -0.298999995f);
                    armRight[SHOULDER].localRotation = Quaternion.Euler(new Vector3(79.2882004f, 171.104309f, 247.552261f));

                    // Elbow
                    armRight[ELBOW].localRotation = Quaternion.Euler(new Vector3(357.572662f, 4.8641696f, 31.2418327f));

                    // Wrist
                    armRight[WRIST].localRotation = Quaternion.Euler(new Vector3(0.0732730925f, 358.082611f, 15.3616076f));

                    // Thumb
                    armRight[THUMB].localRotation = Quaternion.Euler(new Vector3(69.0503616f, 213.864487f, 13.7173643f));
                    #endregion
                }
                break;
            default: { } break;
        }
    }
}
