using UnityEngine;
using System;
using System.Collections.Generic;


public class ShellCasingScript:MonoBehaviour{
    
    public List<AudioClip> sound_shell_bounce;
    public bool collided = false;
    public Vector3 old_pos;
    public float life_time = 0.0f;
    public float glint_delay = 0.0f;
    public float glint_progress = 0.0f;
    Light glint_light;
    
    public void PlaySoundFromGroup(List<AudioClip> group,float volume){
    	int which_shot = UnityEngine.Random.Range(0,group.Count);
    	GetComponent<AudioSource>().PlayOneShot(group[which_shot], volume * Preferences.sound_volume);
    }
    
    public void Start() {
    	old_pos = transform.position;
    	if(transform.Find("light_pos") != null){
    		glint_light = transform.Find("light_pos").GetComponent<Light>();
    		glint_light.enabled = false;
    	}
    }
    
    public void CollisionSound() {
    	if(!collided){
    		collided = true;
    		PlaySoundFromGroup(sound_shell_bounce, 0.3f);
    	}
    }
    
    RaycastHit hit = new RaycastHit();
    public void FixedUpdate() {
    	Rigidbody rigidbody = GetComponent<Rigidbody>();

    	if(rigidbody && !rigidbody.IsSleeping()) {
    		Collider collider = GetComponent<Collider>();
    		if(collider != null && collider.enabled) {
    			life_time += Time.deltaTime;
    			if(Physics.Linecast(old_pos, transform.position, out hit, 1)){
    				transform.position = hit.point;
    				rigidbody.velocity *= -0.3f;
    			}
    			if(life_time > 3.0f){
    				rigidbody.Sleep();
    			}
    		}
    	} else if(rigidbody && glint_light != null) {
    		if(glint_delay == 0.0f){
    			glint_delay = UnityEngine.Random.Range(1.0f,5.0f);
    		}
    		glint_delay = Mathf.Max(0.0f, glint_delay - Time.deltaTime);
    		if(glint_delay == 0.0f){
    			glint_progress = 1.0f;
    		}
    		if(glint_progress > 0.0f){
    			glint_light.enabled = true;
    			glint_light.intensity = Mathf.Sin(glint_progress * Mathf.PI);
    			glint_progress = Mathf.Max(0.0f, glint_progress - Time.deltaTime * 2.0f);
    		} else {
    			glint_light.enabled = false;
    		}
    	}
    	old_pos = transform.position;
    }
    
    public void OnCollisionEnter(Collision collision) {
    	CollisionSound();
    }
}
