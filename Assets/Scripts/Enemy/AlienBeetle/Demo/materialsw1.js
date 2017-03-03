    var myMaterials : Material[];
    var NextMaterial : int = 0;
    function NextMaterialUsed()
    {
    GetComponent.<Renderer>().sharedMaterial = myMaterials[NextMaterial];
    if(NextMaterial < myMaterials.length-1)
    NextMaterial += 1;
    else
    NextMaterial = 0;
    }
     
    function Update ()
    {
    if(Input.GetKeyDown(KeyCode.C))
    {
    NextMaterialUsed();
    }
     
     
    }