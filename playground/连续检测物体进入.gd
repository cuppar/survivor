extends Node2D

func _on_area_2d_body_entered(body: Node2D) -> void:
	print("body enter")


func _on_timer_timeout() -> void:
	print("timer timeout")
	$Area2D/CollisionShape2D.set_deferred("disabled", true)
	$Area2D/CollisionShape2D.set_deferred("disabled", false)
