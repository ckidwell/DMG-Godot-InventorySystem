class_name example_equipment_button
extends Button

var InventoryEvents

func _ready():
	InventoryEvents = get_node("/root/InventoryEvents")
	pressed.connect(_button_pressed)
	
func _button_pressed():
	InventoryEvents.EmitShowEquipment()
	
