using UnityEngine;

public class MG : Gun
{
    float targetAdjust = 0.25f;

    public override void Use()
    {
        Vector3 dir;
        if (currentState == ItemState.Player)
        {
            if (bullets > 0)
            {
                bullets--;
                Vector3 spread = new Vector3(Random.Range(-data.spreadAngle, data.spreadAngle), Random.Range(-data.spreadAngle, data.spreadAngle), 0f);
                Quaternion rotation = Quaternion.Euler(Camera.main.transform.eulerAngles + spread);
                Ray ray = new Ray(firePoint.position, rotation * Vector3.forward);
                dir = ray.direction;

                FireBasic();
                FireRay(dir);

                sound?.MGShot();
            }
            else
            {
                popUp?.UpdatePopUp("EMPTY");
                sound?.ShotEmpty();
            }
        }
        else if (currentState == ItemState.Enemy && holder is Enemy enemy)
        {
            Vector3 spread = new Vector3(Random.Range(-data.spreadAngle, data.spreadAngle), Random.Range(-data.spreadAngle, data.spreadAngle), 0f);
            Quaternion rotation = Quaternion.Euler(spread);

            Vector3 target = enemy.target.transform.position;
            target.y += targetAdjust;
            dir = rotation * (target - firePoint.position).normalized;

            FireBasic();
            FireBullet(dir);

            //sound?.EnemySFX(enemy.sfx, enemy.attackClip);
        }
    }

    void FireBasic()
    {
        Recoil();
        data.muzzleFlash?.Play();
    }
}
