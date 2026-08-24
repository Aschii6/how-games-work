extends CharacterBody2D

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

const SPEED: float = 300.0
const ACCELERATION: float = SPEED * 3

enum State {IDLE, RUN, ATTACK1, ATTACK2, GUARD}
var state: State = State.IDLE

func _physics_process(delta: float) -> void:
	var input_direction := Input.get_vector("left", "right", "up", "down")
	var target_velocity: Vector2 = input_direction * SPEED
	velocity = velocity.move_toward(target_velocity, ACCELERATION * delta)
	
	if velocity.is_zero_approx():
		set_state(State.IDLE)
	else:
		set_state(State.RUN)
		if !is_zero_approx(velocity.x):
			animated_sprite_2d.flip_h = true if velocity.x < 0 else false
	
	move_and_slide()

func set_state(new_state: State) -> void:
	if state == new_state: return
	
	state = new_state
	
	match state:
		State.IDLE:
			animated_sprite_2d.play("idle")
		State.RUN:
			animated_sprite_2d.play("run")
