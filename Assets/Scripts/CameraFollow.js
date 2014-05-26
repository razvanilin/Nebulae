#pragma strict

var target : Transform;
var distance = 3.0;
var height = 3.0;
var damping = 5.0;
var smoothRotation = true;
var rotationDamping = 10.0;

private var heightCorrection : Vector3;

function Start()
{
	
}
 
function FixedUpdate () {

	heightCorrection = transform.position.up * height;
	
    var wantedPosition = target.TransformPoint(0, height, -distance);
    transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);
 
    if (smoothRotation) {
        var wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
        transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
    }
 
    else transform.LookAt (target, target.up);
}