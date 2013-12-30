using UnityEngine;
using System.Collections;





public class PlayerScript : EntityScript {
	
	private EquipmentScript[] _equipSlots;
    public static PlayerScript mSingleton;

    
    
    private WeaponScript weaponScript;
    
    public Transform weaponIdle;

    HelperUtil helper;
    
    
    public void activatePowerUp(PowerUp name)
    {


    }
	
	protected override void Awake()
	{
		//_equipSlots = new EquipmentScript[typeof(EquipType)]	
        base.Awake();

		_equipSlots = new EquipmentScript[7];

        helper = new HelperUtil();

        mSingleton = this;
        shieldReactionTime = 0.2f;

        

        weaponScript = WeaponHand.GetComponent<WeaponScript>();
		
	}

    



    

    public WeaponScript Weapon
    {
        get { return weaponScript; }

    }


	#region getset
	public EquipmentScript HeadSlot
	{
		get{ return _equipSlots[(int)EquipType.head]; }
		set{ _equipSlots[(int)EquipType.head] = value; }
		
	}
	public EquipmentScript Legsslot
	{
		get{ return _equipSlots[(int)EquipType.legs]; }
		set{ _equipSlots[(int)EquipType.legs] = value; }
		
	}
	public EquipmentScript HandsSlot
	{
		get{ return _equipSlots[(int)EquipType.hands]; }
		set{ _equipSlots[(int)EquipType.hands] = value; }
		
	}
	public EquipmentScript ShoulderSlot
	{
		get{ return _equipSlots[(int)EquipType.shoulder]; }
		set{ _equipSlots[(int)EquipType.shoulder] = value; }
		
	}
	public EquipmentScript ShieldSlot
	{
		get{ return _equipSlots[(int)EquipType.shield]; }
		set{ _equipSlots[(int)EquipType.shield] = value; }
		
	}
	public EquipmentScript WeaponSlot
	{
		get{ return _equipSlots[(int)EquipType.weapon]; }
		set{ _equipSlots[(int)EquipType.weapon] = value; }
		
	}
	
	
	
	
	#endregion
	

}
