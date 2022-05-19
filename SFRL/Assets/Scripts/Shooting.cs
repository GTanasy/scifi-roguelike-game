using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Shooting : MonoBehaviour
    {
        public Transform _firePoint;
        public GameObject _bulletPrefab;
        public GameObject _piercingShotPrefab;

        public float _bulletForce = 20f;
        public float _timeBetweenShots = 0.5f;

        bool _readyToShoot = true;       

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButton("Fire1") && _readyToShoot == true)
            {
                Shoot();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                ShootPiercing();
            }

        }

        void Shoot()
        {
            _readyToShoot = false;
            GameObject _bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

            Rigidbody2D _rb = _bullet.GetComponent<Rigidbody2D>();

            _rb.AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);        
            
            _readyToShoot = false;
            Invoke("ResetShot", _timeBetweenShots);
        }
        
        void ResetShot()
        {
            _readyToShoot = true;
        }

        void ShootPiercing()
        {
            GameObject _piercingShot = Instantiate(_piercingShotPrefab, _firePoint.position, _firePoint.rotation);

            Rigidbody2D _rb = _piercingShot.GetComponent<Rigidbody2D>();

            _rb.AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);
        }
}