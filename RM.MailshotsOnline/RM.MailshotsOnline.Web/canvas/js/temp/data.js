// temp data for use during development
define([],
    function() {

    	/**
    	 * View Model containing dummy data for use during development
    	 * @return {function}     viewModel
    	 */
        function tempViewModel() {

            this.formatData = [
                {
                    id: 2,
                    name: 'A5 Card',
                    faces: [
                        {
                            name:'Front',
                            side:'front',
                            width: 849,
                            height: 600,
                            default_face: true
                        },
                        {
                            name:'Back',
                            side:'back',
                            width: 849,
                            height: 600
                        }
                    ]
                },
                {
                    id: 4,
                    name: 'A4 Letter',
                    faces: [
                        {
                            name:'Front',
                            side:'front',
                            width: 849,
                            height: 1200,
                            default_face: true
                        }
                    ]
                },
                {
                    id: 3,
                    name: 'Self Mailer',
                    faces: [
                        {
                            name:'Front',
                            side:'front',
                            width: 849,
                            height: 600,
                            default_face: true
                        },
                        {
                            name:'Back',
                            side:'front',
                            width: 849,
                            height: 600
                        },
                        {
                            name:'Inside',
                            side:'back',
                            width: 849,
                            height: 1200
                        }
                    ]
                }
            ]

            this.templateData = [

                // card templates
                /*{  
                    id:4,
                    format_id:2,
                    name:"Card Template 1",
                    elements:[  
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"90px"
                                }
                            ]
                        },
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"400px"
                                },
                                {  
                                    property:"width",
                                    value:"449px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {  
                            name:"greeting",
                            face:"Front",
                            type:"html",
                            theme_class:"greeting",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"160px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"30px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"210px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"170px"
                                }
                            ]
                        },
                        {  
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {  
                            name:"cta",
                            face:"Front",
                            type:"html",
                            theme_class:"cta",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"480px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"20px"
                                },
                                {
                                    property:"right",
                                    value:"30px"
                                },
                                {
                                    property:"width",
                                    value:"150px"
                                },
                                {
                                    property:"height",
                                    value:"150px"
                                },
                                {
                                    property:"z-index",
                                    value:"110"
                                }
                            ],
                            message:"Write about an offer here."
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"90px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"150px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"260px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"190px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"460px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"540px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        }
                    ]
                },
                {  
                    id:5,
                    format_id:2,
                    name:"Card Template 2",
                    elements:[  
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"90px"
                                }
                            ]
                        },
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"400px"
                                },
                                {  
                                    property:"width",
                                    value:"449px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {  
                            name:"greeting",
                            face:"Front",
                            type:"html",
                            theme_class:"greeting",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"160px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"30px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"210px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"170px"
                                }
                            ]
                        },
                        {  
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {  
                            name:"cta",
                            face:"Front",
                            type:"html",
                            theme_class:"cta",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"480px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"330px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"90px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"150px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"260px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"190px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"460px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"540px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        }
                    ]
                },
                {  
                    id:6,
                    format_id:2,
                    name:"Card Template 3",
                    elements:[
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"420px"
                                }
                            ]
                        },
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"30px"
                                },
                                {  
                                    property:"right",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"610px"
                                },
                                {  
                                    property:"height",
                                    value:"130px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"20px"
                                },
                                {
                                    property:"right",
                                    value:"30px"
                                },
                                {
                                    property:"width",
                                    value:"150px"
                                },
                                {
                                    property:"height",
                                    value:"150px"
                                },
                                {
                                    property:"z-index",
                                    value:"110"
                                }
                            ],
                            message:"Write about an offer here."
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"90px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"150px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"260px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"190px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"460px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"540px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        }
                    ]
                },
                {  
                    id:7,
                    format_id:2,
                    name:"Card Template 4",
                    elements:[
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"420px"
                                }
                            ]
                        },
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"30px"
                                },
                                {  
                                    property:"right",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"440px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"610px"
                                },
                                {  
                                    property:"height",
                                    value:"130px"
                                }
                            ]
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"90px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"150px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"260px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"190px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"460px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"540px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"400px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        }
                    ]
                },
                {  
                    id:8,
                    format_id:2,
                    name:"Card Template 5",
                    elements:[
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {  
                            name:"back_hero_image",
                            face:"Back",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        }
                    ]
                }*/

                // self mailer templates
                /*{  
                    id:9,
                    format_id:3,
                    name:"Self Mailer Template 1",
                    elements: [
                        {
                            name:"inside_hero",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"540px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Inside",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"540px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"660px"
                                }
                            ] 
                        },
                        {
                            name:"thinbox",
                            face:"Inside",
                            type:"box",
                            theme_class:"thintopborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"1090px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ] 
                        },
                        {  
                            name:"inside_headline",
                            face:"Inside",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"620px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"inside_subline",
                            face:"Inside",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"730px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"30px"
                                }
                            ]
                        },
                        {  
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"790px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"459px"
                                },
                                {  
                                    property:"height",
                                    value:"240px"
                                }
                            ]
                        },
                        {  
                            name:"inside_cta",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_cta",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"790px"
                                },
                                {  
                                    property:"left",
                                    value:"539px"
                                },
                                {  
                                    property:"width",
                                    value:"250px"
                                },
                                {  
                                    property:"height",
                                    value:"240px"
                                }
                            ]
                        },
                        {  
                            name:"inside_footer_1",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"1110px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"220px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {  
                            name:"inside_footer_2",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"1110px"
                                },
                                {  
                                    property:"left",
                                    value:"280px"
                                },
                                {  
                                    property:"width",
                                    value:"220px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {  
                            name:"inside_logo",
                            face:"Inside",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"1110px"
                                },
                                {  
                                    property:"left",
                                    value:"590px"
                                },
                                {  
                                    property:"width",
                                    value:"220px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name:"fold",
                            face:"Inside",
                            type:"fold",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"599px"
                                },
                                {
                                    property:"left",
                                    value:"0"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                }
                            ]
                        },
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"440px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Front",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"440px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"160px"
                                }
                            ] 
                        },
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"15px"
                                },
                                {  
                                    property:"right",
                                    value:"25px"
                                },
                                {  
                                    property:"width",
                                    value:"240px"
                                },
                                {  
                                    property:"height",
                                    value:"115px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"470px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"560px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"30px"
                                },
                                {
                                    property:"right",
                                    value:"30px"
                                },
                                {
                                    property:"width",
                                    value:"140px"
                                },
                                {
                                    property:"height",
                                    value:"140px"
                                },
                                {
                                    property:"z-index",
                                    value:"110"
                                }
                            ],
                            message:"Write about an offer here."
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"30px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"250px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"110px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"220px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"170px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta_closer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Back",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ] 
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"555px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"35px"
                                }
                            ]
                        }
                    ]
                },
                {  
                    id:10,
                    format_id:3,
                    name:"Self Mailer Template 2",
                    elements: [
                        {
                            name:"inside_image_1",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"500px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_1",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"710px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_2",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"500px"
                                },
                                {  
                                    property:"left",
                                    value:"303px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_2",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"710px"
                                },
                                {  
                                    property:"left",
                                    value:"303px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_3",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"500px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_3",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"710px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_4",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"770px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_4",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"980px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_5",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"770px"
                                },
                                {  
                                    property:"left",
                                    value:"303px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_5",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"980px"
                                },
                                {  
                                    property:"left",
                                    value:"303px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_6",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"770px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_6",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"980px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {  
                            name:"inside_headline",
                            face:"Inside",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {  
                            name:"inside_subline",
                            face:"Inside",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"260px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {  
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"340px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"140px"
                                }
                            ]
                        },
                        {  
                            name:"inside_cta",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_cta_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"60px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"inside_footer",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_footer_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name:"fold",
                            face:"Inside",
                            type:"fold",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"599px"
                                },
                                {
                                    property:"left",
                                    value:"0"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                }
                            ]
                        },
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"440px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Front",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"440px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"160px"
                                }
                            ] 
                        },
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"15px"
                                },
                                {  
                                    property:"right",
                                    value:"25px"
                                },
                                {  
                                    property:"width",
                                    value:"240px"
                                },
                                {  
                                    property:"height",
                                    value:"115px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"470px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"560px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"30px"
                                },
                                {
                                    property:"right",
                                    value:"30px"
                                },
                                {
                                    property:"width",
                                    value:"140px"
                                },
                                {
                                    property:"height",
                                    value:"140px"
                                },
                                {
                                    property:"z-index",
                                    value:"110"
                                }
                            ],
                            message:"Write about an offer here."
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"30px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"250px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"110px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"220px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"170px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta_closer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Back",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ] 
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"555px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"35px"
                                }
                            ]
                        }
                    ]
                },
                {  
                    id:11,
                    format_id:3,
                    name:"Self Mailer Template 3",
                    elements: [
                        {
                            name:"inside_hero",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"400px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_1",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"770px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_1",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"980px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_2",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"770px"
                                },
                                {  
                                    property:"left",
                                    value:"303px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_2",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"980px"
                                },
                                {  
                                    property:"left",
                                    value:"303px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_3",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"770px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_3",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"980px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {  
                            name:"inside_headline",
                            face:"Inside",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"430px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"inside_subline",
                            face:"Inside",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"530px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {  
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"590px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"150px"
                                }
                            ]
                        },
                        {  
                            name:"inside_cta",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_cta_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"60px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"fold",
                            face:"Inside",
                            type:"fold",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"599px"
                                },
                                {
                                    property:"left",
                                    value:"0"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                }
                            ]
                        },
                        {  
                            name:"inside_footer",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_footer_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"440px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Front",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"440px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"160px"
                                }
                            ] 
                        },
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"15px"
                                },
                                {  
                                    property:"right",
                                    value:"25px"
                                },
                                {  
                                    property:"width",
                                    value:"240px"
                                },
                                {  
                                    property:"height",
                                    value:"115px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"470px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"560px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"30px"
                                },
                                {
                                    property:"right",
                                    value:"30px"
                                },
                                {
                                    property:"width",
                                    value:"140px"
                                },
                                {
                                    property:"height",
                                    value:"140px"
                                },
                                {
                                    property:"z-index",
                                    value:"110"
                                }
                            ],
                            message:"Write about an offer here."
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"30px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"250px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"110px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"220px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"170px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta_closer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Back",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ] 
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"555px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"35px"
                                }
                            ]
                        }
                    ]
                },
                {  
                    id:12,
                    format_id:3,
                    name:"Self Mailer Template 4",
                    elements: [
                        {
                            name:"inside_image_1",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"230px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_1",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"440px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_2",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"500px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_2",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"710px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"inside_image_3",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"770px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"200px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_3",
                            face:"Inside",
                            type:"html",
                            theme_class:"caption",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"980px"
                                },
                                {  
                                    property:"left",
                                    value:"566px"
                                },
                                {  
                                    property:"width",
                                    value:"243px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {  
                            name:"inside_headline",
                            face:"Inside",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"769px"
                                },
                                {  
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {  
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"230px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"496px"
                                },
                                {  
                                    property:"height",
                                    value:"790px"
                                }
                            ]
                        },
                        {  
                            name:"inside_cta",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_cta_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"60px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"inside_footer",
                            face:"Inside",
                            type:"html",
                            theme_class:"inside_footer_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name:"fold",
                            face:"Inside",
                            type:"fold",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"599px"
                                },
                                {
                                    property:"left",
                                    value:"0"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                }
                            ]
                        },
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"440px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Front",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"440px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"160px"
                                }
                            ] 
                        },
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"15px"
                                },
                                {  
                                    property:"right",
                                    value:"25px"
                                },
                                {  
                                    property:"width",
                                    value:"240px"
                                },
                                {  
                                    property:"height",
                                    value:"115px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"470px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {  
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"560px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"30px"
                                },
                                {
                                    property:"right",
                                    value:"30px"
                                },
                                {
                                    property:"width",
                                    value:"140px"
                                },
                                {
                                    property:"height",
                                    value:"140px"
                                },
                                {
                                    property:"z-index",
                                    value:"110"
                                }
                            ],
                            message:"Write about an offer here."
                        },
                        {  
                            name:"back_logo",
                            face:"Back",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"30px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"250px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {  
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"back_headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"110px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"back_body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"220px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"170px"
                                }
                            ]
                        },
                        {  
                            name:"back_cta",
                            face:"Back",
                            type:"html",
                            theme_class:"cta_closer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Back",
                            type:"box",
                            theme_class:"topborderbox",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                }
                            ] 
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"footer",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"555px"
                                },
                                {  
                                    property:"left",
                                    value:"30px"
                                },
                                {  
                                    property:"width",
                                    value:"420px"
                                },
                                {  
                                    property:"height",
                                    value:"35px"
                                }
                            ]
                        }
                    ]
                }, 
                {  
                    id:13,
                    format_id:3,
                    name:"Self Mailer Template 5",
                    elements:[
                        {  
                            name:"hero_image",
                            face:"Front",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {  
                            name:"back_hero_image",
                            face:"Back",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"520px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        },
                        {  
                            name:"inside_hero_image",
                            face:"Inside",
                            type:"image",
                            theme_class:"hero",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"1200px"
                                }
                            ]
                        },
                        {
                            name:"fold",
                            face:"Inside",
                            type:"fold",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"599px"
                                },
                                {
                                    property:"left",
                                    value:"0"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"520px"
                                },
                                {  
                                    property:"width",
                                    value:"329px"
                                },
                                {  
                                    property:"height",
                                    value:"600px"
                                }
                            ]
                        }
                    ]
                }*/

                // letter templates
                {  
                    id:14,
                    format_id:4,
                    name:"Letter Template 1",
                    elements:[
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Front",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"200px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"180px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Front",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"520px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"550px"
                                }
                            ]
                        },
                        {  
                            name:"return_address",
                            face:"Front",
                            type:"html",
                            theme_class:"return_address",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"right",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"150px"
                                }
                            ]
                        },
                        {  
                            name:"footer",
                            face:"Front",
                            type:"html",
                            theme_class:"footer_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                    ]
                },
                {  
                    id:15,
                    format_id:4,
                    name:"Letter Template 2",
                    elements:[
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Front",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"200px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"180px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"600px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Front",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"520px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"600px"
                                },
                                {  
                                    property:"height",
                                    value:"360px"
                                }
                            ]
                        },
                        {
                            name:"sign_off",
                            face:"Front",
                            type:"html",
                            theme_class:"sign_off",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"910px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
                        {  
                            name:"signature",
                            face:"Front",
                            type:"image",
                            theme_class:"signature",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"940px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name:"printed_name",
                            face:"Front",
                            type:"html",
                            theme_class:"printed_name",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"1045px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"post_script",
                            face:"Front",
                            type:"html",
                            theme_class:"post_script",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"1100px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"600px"
                                },
                                {  
                                    property:"height",
                                    value:"30px"
                                }
                            ]
                        },
                        {  
                            name:"return_address",
                            face:"Front",
                            type:"html",
                            theme_class:"return_address",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"right",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"150px"
                                }
                            ]
                        },
                        {  
                            name:"footer",
                            face:"Front",
                            type:"html",
                            theme_class:"footer_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                    ]
                },
                {  
                    id:16,
                    format_id:4,
                    name:"Letter Template 3",
                    elements:[
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Front",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"200px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"180px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Front",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"520px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"450px"
                                }
                            ]
                        },
                        {  
                            name:"return_address",
                            face:"Front",
                            type:"html",
                            theme_class:"return_address",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"right",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"150px"
                                }
                            ]
                        },
                        {  
                            name:"cta",
                            face:"Front",
                            type:"html",
                            theme_class:"cta_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"60px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"120px"
                                }
                            ]
                        },
                        {  
                            name:"footer",
                            face:"Front",
                            type:"html",
                            theme_class:"footer_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                    ]
                },
                {  
                    id:17,
                    format_id:4,
                    name:"Letter Template 2",
                    elements:[
                        {  
                            name:"logo",
                            face:"Front",
                            type:"image",
                            theme_class:"logo",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"left",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"100px"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Front",
                            type:"noprint",
                            theme_class:"noprint",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"200px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"550px"
                                },
                                {  
                                    property:"height",
                                    value:"180px"
                                }
                            ]
                        },
                        {  
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"400px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"600px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Front",
                            type:"html",
                            theme_class:"body",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"480px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"600px"
                                },
                                {  
                                    property:"height",
                                    value:"300px"
                                }
                            ]
                        },
                        {
                            name:"sign_off",
                            face:"Front",
                            type:"html",
                            theme_class:"sign_off",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"800px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
                        {  
                            name:"signature",
                            face:"Front",
                            type:"image",
                            theme_class:"signature",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"830px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"printed_name",
                            face:"Front",
                            type:"html",
                            theme_class:"printed_name",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"915px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"200px"
                                },
                                {  
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name:"post_script",
                            face:"Front",
                            type:"html",
                            theme_class:"post_script",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"970px"
                                },
                                {  
                                    property:"left",
                                    value:"80px"
                                },
                                {  
                                    property:"width",
                                    value:"600px"
                                },
                                {  
                                    property:"height",
                                    value:"30px"
                                }
                            ]
                        },
                        {  
                            name:"return_address",
                            face:"Front",
                            type:"html",
                            theme_class:"return_address",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"top",
                                    value:"40px"
                                },
                                {  
                                    property:"right",
                                    value:"40px"
                                },
                                {  
                                    property:"width",
                                    value:"150px"
                                },
                                {  
                                    property:"height",
                                    value:"150px"
                                }
                            ]
                        },
                        {  
                            name:"cta",
                            face:"Front",
                            type:"html",
                            theme_class:"cta_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"60px"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"120px"
                                }
                            ]
                        },
                        {  
                            name:"footer",
                            face:"Front",
                            type:"html",
                            theme_class:"footer_block",
                            layout:[  
                                {  
                                    property:"position",
                                    value:"absolute"
                                },
                                {  
                                    property:"bottom",
                                    value:"0"
                                },
                                {  
                                    property:"left",
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"849px"
                                },
                                {  
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                    ]
                }
            ]

            this.themeData = [
                {
                    id: 3,
                    name: 'Fashion',
                    faces: [
                    	{
                    		name: 'Front',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#4B4C4E'
			                    },
                    		]
                    	},
                    	{
                    		name: 'Back',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#4B4C4E'
			                    }
                    		]
                    	},
                        {
                            name: 'Inside',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#4B4C4E'
                                }
                            ]
                        }
                        
                    ],
                    colours: ['#D4E5E6', 'rgb(56, 63, 87)', 'rgb(50, 132, 134)', 'rgb(90, 93, 93)', '#FFF' ],
                    fonts: [
                    	{
                    		name: 'Merriweather',
                    		value: "'Merriweather', serif"
                    	},
                    	{
                    		name: 'Roboto',
                    		value: "'Roboto', serif"
                    	}
                    ],
                    classes: [
                        {
                            name: 'roundel',
                            font_sizes: ['18px', '19px', '20px', '21px', '22px', '23px', '24px', '25px', '26px', '27px', '28px', '29px', '30px', '31px', '32px', '33px', '34px', '35px', '36px', '37px', '38px', '39px', '40px', '41px', '42px', '43px', '44px', '45px', '46px', '48px', '50px', '52px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'center'
                                },
                                {
                                    property: 'font-size',
                                    value: '48px'
                                },
                                {
                                    property: 'font-weight',
                                    value: 300
                                },
                                {
                                    property: 'color',
                                    value: '#4B4C4E'
                                },
                                {
                                    property: 'background-color',
                                    value: '#A0D0AA'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Merriweather', serif"
                                },
                                {
                                    property: 'font-style',
                                    value: 'italic'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'bold'
                                },
                                {
                                    property: 'border-radius',
                                    value: "50%"
                                },
                                {
                                    property: '-moz-border-radius',
                                    value: "50%"
                                },
                                {
                                    property: '-webkit-border-radius',
                                    value: "50%"
                                },
                                {
                                    property: 'padding',
                                    value: '0 15px'
                                },
                                /*{
                                    property: 'transform',
                                    value: 'rotate(-15deg)'
                                },
                                {
                                    property: 'line-height',
                                    value: '110%;'
                                }*/
                            ]
                        },
                        {
                            name: 'topborderbox',
                            styles: [
                                {
                                    property: 'border-top',
                                    value: '20px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#A0D0AA'
                                },
                                {
                                    property: 'background-color',
                                    value: '#4B4C4E'
                                }
                            ]
                        },
                        {
                            name: 'inside_cta_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#A0D0AA'
                                }
                            ]
                        },
                        {
                            name: 'cta_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#A0D0AA'
                                }
                            ]
                        },
                        {
                            name: 'inside_footer_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#4B4C4E'
                                }
                            ]
                        },
                        {
                            name: 'footer_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#4B4C4E'
                                }
                            ]
                        },
                        {
                            name: 'thintopborderbox',
                            styles: [
                                {
                                    property: 'border-top',
                                    value: '2px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#A0D0AA'
                                }
                            ]
                        },
                        {
                            name: 'inside_cta',
                            font_sizes: ['16px', '17px', '18px', '19px', '20px', '21px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#A0D0AA'
                                },
                                {
                                    property: 'color',
                                    value: '#A0D0AA'
                                },
                                {
                                    property: 'text-align',
                                    value: 'center'
                                },
                                {
                                    property: 'padding',
                                    value: '15px'
                                },
                                {
                                    property: 'font-size',
                                    value: '20px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                },
                            ]
                        },
                        {
                            name: 'greeting',
                            font_sizes: ['16px', '17px', '18px', '19px', '20px', '21px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '18px'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'headline',
                            font_sizes: ['22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px', '38px', '40px', '42px', '44px', '46px', '48px', '50px', '52px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '48px'
                                },
                                {
                                	property: 'color',
                                	value: '#A0D0AA'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Merriweather', serif"
                                },
                                {
                                    property: 'font-style',
                                    value: 'italic'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'bold'
                                }
                            ]
                        },
                        {
                            name: 'back_headline',
                            font_sizes: ['22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px', '38px', '40px', '42px', '44px', '46px', '48px', '50px', '52px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '48px'
                                },
                                {
                                    property: 'color',
                                    value: '#A0D0AA'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Merriweather', serif"
                                },
                                {
                                    property: 'font-style',
                                    value: 'italic'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'bold'
                                }
                            ]
                        },
                        {
                            name: 'subline',
                            font_sizes: ['16px', '17px', '18px', '19px', '20px', '21px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '18px'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'body',
                            font_sizes: ['8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                	property: 'font-size',
                                	value: '15px'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Roboto', serif"
                                },
                                {
                                	property: 'text-align',
                                	value: 'left'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'back_body',
                            font_sizes: ['8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-size',
                                    value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'inside_footer',
                            font_sizes: ['8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-size',
                                    value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
    					{
                            name: 'cta',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#4B4C4E'
                                },
                                {
                                    property: 'background-color',
                                    value: '#A0D0AA'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                	property: 'padding',
                                	value: '11px 11px 11px 40px'
                                },
                                {
                                	property: 'font-size',
                                	value: '20px'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Roboto', serif"
                                }
                                ,
                                {
                                    property: 'margin-left',
                                    value: '-40px'
                                }
                            ]
                        },
                        {
                            name: 'cta_closer',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#4B4C4E'
                                },
                                {
                                    property: 'background-color',
                                    value: '#A0D0AA'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'padding',
                                    value: '11px 11px 11px 30px'
                                },
                                {
                                    property: 'font-size',
                                    value: '20px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                },
                                {
                                    property: 'margin-left',
                                    value: '-30px'
                                }
                            ]
                        },
                        {
                            name: 'footer',
                            font_sizes: ['10px', '12px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-size',
                                    value: '12px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'logo',
                            src: '',
                            img_position: {
                                top: 0,
                                left: 0
                            },
                            scale: 100
                        }
                    ]
                },
                {
                    id: 4,
                    name: 'Trusted / Traditional',
                    faces: [
                    	{
                    		name: 'Front',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#371B4C'
			                    }
                    		]
                    	},
                    	{
                    		name: 'Back',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#FFFFFF'
			                    }
                    		]
                    	},
                        {
                            name: 'Inside',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#FFFFFF'
                                }
                            ]
                        }
                    ],
                    fonts: [
                        {
                            name: 'Domine',
                            value: "'Domine', serif"
                        },
                        {
                            name: 'Lato',
                            value: "'Lato', sans-serif"
                        }
                    ],
                    colours: ['rgb(224, 251, 255)', 'rgb(102, 154, 185)', 'rgb(69, 71, 81)' ],
                    classes: [
                        {
                            name: 'roundel',
                            font_sizes: ['18px', '19px', '20px', '21px', '22px', '23px', '24px', '25px', '26px', '27px', '28px', '29px', '30px', '31px', '32px', '33px', '34px', '35px', '36px', '37px', '38px', '39px', '40px', '41px', '42px', '43px', '44px', '45px', '46px', '48px', '50px', '52px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'center'
                                },
                                {
                                    property: 'font-size',
                                    value: '19px'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'background-color',
                                    value: '#4C863D'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                },
                                {
                                    property: 'border-radius',
                                    value: "50%"
                                },
                                {
                                    property: '-moz-border-radius',
                                    value: "50%"
                                },
                                {
                                    property: '-webkit-border-radius',
                                    value: "50%"
                                },
                                {
                                    property: 'padding',
                                    value: '0 15px'
                                },
                                {
                                    property: 'line-height',
                                    value: '110%;'
                                }
                            ]
                        },
                        {
                            name: 'topborderbox',
                            styles: [
                                {
                                    property: 'border-top',
                                    value: '20px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4C863D'
                                },
                                {
                                    property: 'background-color',
                                    value: '#371B4C'
                                }
                            ]
                        },
                        {
                            name: 'inside_cta_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#4C863D'
                                }
                            ]
                        },
                        {
                            name: 'cta_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#4C863D'
                                }
                            ]
                        },
                        {
                            name: 'inside_footer_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#371B4C'
                                }
                            ]
                        },
                        {
                            name: 'inside_footer',
                            font_sizes: ['8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-size',
                                    value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'footer_block',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#371B4C'
                                }
                            ]
                        },
                        {
                            name: 'footer',
                            font_sizes: ['8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-size',
                                    value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'thintopborderbox',
                            styles: [
                                {
                                    property: 'border-top',
                                    value: '2px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4C863D'
                                }
                            ]
                        },
                        {
                            name: 'greeting',
                            font_sizes: ['16px', '17px', '18px', '19px', '20px', '21px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '18px'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'headline',
                            font_sizes: ['22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px', '38px', '40px', '42px', '44px', '46px', '48px', '50px', '52px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '48px'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                }
                            ]
                        },
                        {
                            name: 'back_headline',
                            font_sizes: ['22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px', '38px', '40px', '42px', '44px', '46px', '48px', '50px', '52px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '48px'
                                },
                                {
                                    property: 'color',
                                    value: '#371B4C'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                },
                                {
                                    property: 'font-weight',
                                    value: 'bold'
                                }
                            ]
                        },
                        {
                            name: 'subline',
                            font_sizes: ['12px', '13px', '14px', '15px', '16px', '17px', '18px', '19px', '20px', '21px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '18px'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                },
                                {
                                    property: 'font-weight',
                                    value: 'lighter'
                                }
                            ]
                        },
                        {
                            name: 'body',
                            font_sizes: ['10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                	property: 'font-weight',
                                	value: 400
                                },
                                {
                                	property: 'font-size',
                                	value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'back_body',
                            font_sizes: ['10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#000000'
                                },
                                {
                                    property: 'font-weight',
                                    value: 400
                                },
                                {
                                    property: 'font-size',
                                    value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'inside_cta',
                            font_sizes: ['16px', '17px', '18px', '19px', '20px', '21px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4C863D'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'text-align',
                                    value: 'center'
                                },
                                {
                                    property: 'padding',
                                    value: '15px'
                                },
                                {
                                    property: 'font-size',
                                    value: '26px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                }
                            ]
                        },
    					{
                            name: 'cta',
                            font_sizes: ['10px', '12px', '14px', '16px', '17px', '18px', '19px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4C863D'
                                },
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'padding',
                                    value: '10px'
                                },
                                {
                                    property: 'font-size',
                                    value: '26px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                }
                            ]
                        },
                        {
                            name: 'back_cta',
                            font_sizes: ['10px', '12px', '14px', '16px', '17px', '18px', '19px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#371B4C'
                                },
                                {
                                    property: 'color',
                                    value: '#4C863D'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'padding',
                                    value: '5px'
                                },
                                {
                                    property: 'font-size',
                                    value: '26px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                }
                            ]
                        },
                        {
                            name: 'cta_closer',
                            font_sizes: ['16px', '17px', '18px', '19px', '20px', '21px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#371B4C'
                                },
                                {
                                    property: 'color',
                                    value: '#4C863D'
                                },
                                {
                                    property: 'text-align',
                                    value: 'center'
                                },
                                {
                                    property: 'padding',
                                    value: '15px'
                                },
                                {
                                    property: 'font-size',
                                    value: '26px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                }
                            ]
                        },
                        {
                            name: 'logo',
                            src: '',
                            img_position: {
                                top: 0,
                                left: 0
                            },
                            scale: 100
                        }
                    ]
                },
                {
                    id: 5,
                    name: 'Test Theme 3',
                    faces: [
                    	{
                    		name: 'Front',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: 'rgb(255, 158, 50)'
			                    }
                    		]
                    	},
                    	{
                    		name: 'Back',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: 'rgb(255, 158, 50)'
			                    }
                    		]
                    	},
                        {
                            name: 'Inside',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: 'rgb(255, 158, 50)'
                                }
                            ]
                        }
                    ],
                    fonts: [
                        {
                            name: 'Dosis',
                            value: "'Fredoka One', cursive"
                        },
                        {
                            name: 'Arial',
                            value: 'Arial'
                        }
                    ],
                    colours: ['rgb(255, 158, 50)', 'rgb(253, 233, 95)', 'rgb(246, 125, 40)' ],
                    classes: [
                        {
                            name: 'heading',
                            font_sizes: ['22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px', '38px', '40px', '42px', '44px', '45px', '48px', '50px', '52px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '45px'
                                },
                                {
                                	property: 'color',
                                	value: 'rgb(253, 233, 95)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
                        {
                            name: 'heading-large',
                            font_sizes: ['30px', '32px', '34px', '36px', '38px', '40px', '42px', '44px', '46px', '48px', '50px', '52px', '56px', '60px', '64px', '68px', '70px', '74px', '80px', '84px', '90px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '74px'
                                },
                                {
                                    property: 'font-weight',
                                    value: 400
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(253, 233, 95)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
                        {
                            name: 'subheading',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '17px'
                                },
                                {
                                	property: 'color',
                                	value: 'rgb(253, 233, 95)'
                                },
                                {
                                	property: 'border-bottom',
                                	value: '6px dotted rgb(246, 125, 40)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
                        {
                            name: 'subheading-large',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '28px'
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(253, 233, 95)'
                                },
                                {
                                    property: 'border-bottom',
                                    value: '6px dotted rgb(246, 125, 40)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
                        {
                            name: 'subheading-noline',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'font-size',
                                    value: '17px'
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(253, 233, 95)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
                        {
                            name: 'body',
                            font_sizes: ['10px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFF'
                                },
                                {
                                	property: 'font-size',
                                	value: '13px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
    					{
                            name: 'cta',
                            font_sizes: ['10px', '12px', '14px', '16px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: 'rgb(252, 253, 254)'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                	property: 'padding',
                                	value: '16px'
                                },
                                {
                                	property: 'font-size',
                                	value: '20px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
                        {
                            name: 'cta-bigger',
                            font_sizes: ['10px', '12px', '14px', '16px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: 'rgb(252, 253, 254)'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'padding',
                                    value: '24px 16px'
                                },
                                {
                                    property: 'font-size',
                                    value: '20px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Fredoka One', cursive"
                                }
                            ]
                        },
                        {
                            name: 'logo',
                            src: '',
                            img_position: {
                                top: 0,
                                left: 0
                            },
                            scale: 100
                        },
                        {
                            name: 'background',
                            src: 'file:///C:/Users/ext-jlovatt/work/RM.MailshotsOnlineFrontEnd/HC.RM.MailshotsOnline/images/little_cute_cat_1920x1080.jpg',
                            img_position: {
                                top: 0,
                                left: 0
                            },
                            scale: 100
                        }
                    ]
                }
            ]

            this.userData = {
                formatID: 3,
                templateID: 9,
                themeID: 3,
                faces: [],
                elements: []
            }


        }

        return new tempViewModel();
    }
)