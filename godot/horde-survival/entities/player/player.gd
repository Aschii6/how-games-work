extends CharacterBody2D

const SPEED = 300.0
const ACCELERATION = SPEED * 3

func _physics_process(delta: float) -> void:
	var input_direction := Input.get_vector("left", "right", "up", "down")
	var target_velocity = input_direction * SPEED
	velocity = velocity.move_toward(target_velocity, ACCELERATION * delta)
	move_and_slide()
