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
                {  
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"370px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                            theme_class:"headline",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"440px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"370px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                            theme_class:"headline",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"440px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                            theme_class:"headline",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"440px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                            theme_class:"headline",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                    value:"0"
                                },
                                {  
                                    property:"width",
                                    value:"440px"
                                },
                                {  
                                    property:"height",
                                    value:"70px"
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
                                }
                            ]
                        }
                    ]
                }

                // self mailer templates
                /*{  
                    id:5,
                    format_id:3,
                    name:"Self Mailer Template 1",
                    elements: [
                        {
                            name:"inside_image",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                            theme_class:"headline",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
                                }
                            ]
                        },
                        {  
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body",
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
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
                                },
                                {  
                                    property:"outline",
                                    value:"1px solid #666"
                                }
                            ]
                        }
                    ]
                } */ 
            ]

            this.themeData = [
                {
                    id: 3,
                    name: 'Test Theme',
                    faces: [
                    	{
                    		name: 'Front',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#FFFFFF'
			                    },
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
                    colours: ['#D4E5E6', 'rgb(56, 63, 87)', 'rgb(50, 132, 134)', 'rgb(90, 93, 93)', '#FFF' ],
                    fonts: [
                    	{
                    		name: 'Dosis',
                    		value: "'Dosis', sans-serif"
                    	},
                    	{
                    		name: 'Arial',
                    		value: 'Arial'
                    	}
                    ],
                    classes: [
                        {
                            name: 'roundel',
                            font_sizes: ['22px', '23px', '24px', '25px', '26px', '27px', '28px', '29px', '30px', '31px', '32px', '33px', '34px', '35px', '36px', '37px', '38px', '39px', '40px', '41px', '42px', '43px', '44px', '45px', '46px', '48px', '50px', '52px'],
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
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'background-color',
                                    value: 'rgb(18, 115, 10)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Dosis', sans-serif"
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
                                    value: '25px 15px'
                                },
                                {
                                    property: 'transform',
                                    value: 'rotate(10deg)'
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
                            name: 'inside_cta',
                            styles: [
                                {
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4C863D'
                                }
                            ]
                        },
                        {
                            name: 'heading',
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
                                	property: 'font-weight',
                                	value: 300
                                },
                                {
                                	property: 'color',
                                	value: 'rgb(56, 63, 87)'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Dosis', sans-serif"
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
                                    value: 300
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(56, 63, 87)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Dosis', sans-serif"
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
                                	property: 'font-weight',
                                	value: 600
                                },
                                {
                                	property: 'color',
                                	value: 'rgb(50, 132, 134)'
                                },
                                {
                                	property: 'border-bottom',
                                	value: '2px dashed rgb(50, 132, 134)'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Dosis', sans-serif"
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
                                    value: '30px'
                                },
                                {
                                    property: 'font-weight',
                                    value: 600
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(50, 132, 134)'
                                },
                                {
                                    property: 'border-bottom',
                                    value: '2px dashed rgb(50, 132, 134)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Dosis', sans-serif"
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
                                    property: 'font-weight',
                                    value: 600
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(50, 132, 134)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Dosis', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'body',
                            font_sizes: ['10px', '12px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: 'rgb(90, 93, 93)'
                                },
                                {
                                	property: 'font-weight',
                                	value: 500
                                },
                                {
                                	property: 'font-size',
                                	value: '15px'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Dosis', sans-serif"
                                },
                                {
                                	property: 'text-align',
                                	value: 'left'
                                }
                            ]
                        },
    					{
                            name: 'cta',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: 'rgb(50, 132, 134)'
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
                                	property: 'font-weight',
                                	value: 600
                                },
                                {
                                	property: 'font-size',
                                	value: '17px'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Dosis', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'cta-bigger',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: 'rgb(50, 132, 134)'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'border',
                                    value: '2px dashed rgb(50, 132, 134)'
                                },
                                {
                                    property: 'padding',
                                    value: '24px 16px'
                                },
                                {
                                    property: 'font-weight',
                                    value: 600
                                },
                                {
                                    property: 'font-size',
                                    value: '17px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Dosis', sans-serif"
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
                },
                {
                    id: 4,
                    name: 'Test Theme 2',
                    faces: [
                    	{
                    		name: 'Front',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: 'rgb(102, 154, 185)'
			                    }
                    		]
                    	},
                    	{
                    		name: 'Back',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: 'rgb(102, 154, 185)'
			                    }
                    		]
                    	},
                        {
                            name: 'Inside',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: 'rgb(102, 154, 185)'
                                }
                            ]
                        }
                    ],
                    fonts: [
                        {
                            name: 'Dosis',
                            value: "'Lusitana', serif"
                        },
                        {
                            name: 'Arial',
                            value: 'Arial'
                        }
                    ],
                    colours: ['rgb(224, 251, 255)', 'rgb(102, 154, 185)', 'rgb(69, 71, 81)' ],
                    classes: [
                        {
                            name: 'heading',
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
                                	property: 'font-weight',
                                	value: 400
                                },
                                {
                                	property: 'color',
                                	value: 'rgb(224, 251, 255)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lusitana', serif"
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
                                    value: 'rgb(224, 251, 255)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lusitana', serif"
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
                                	property: 'font-weight',
                                	value: 700
                                },
                                {
                                	property: 'color',
                                	value: 'rgb(69, 71, 81)'
                                },
                                {
                                	property: 'border-bottom',
                                	value: '2px solid rgb(224, 251, 255)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lusitana', serif"
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
                                    value: '30px'
                                },
                                {
                                    property: 'font-weight',
                                    value: 700
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(69, 71, 81)'
                                },
                                {
                                    property: 'border-bottom',
                                    value: '2px solid rgb(224, 251, 255)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lusitana', serif"
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
                                    property: 'font-weight',
                                    value: 700
                                },
                                {
                                    property: 'color',
                                    value: 'rgb(69, 71, 81)'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lusitana', serif"
                                }
                            ]
                        },
                        {
                            name: 'body',
                            font_sizes: ['10px', '12px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: 'rgb(252, 253, 254)'
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
                                    value: "'Lusitana', serif"
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
                                	property: 'border',
                                	value: '2px solid rgb(252, 253, 254)'
                                },
                                {
                                	property: 'padding',
                                	value: '16px'
                                },
                                {
                                	property: 'font-weight',
                                	value: 700
                                },
                                {
                                	property: 'font-size',
                                	value: '16px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lusitana', serif"
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
                                    property: 'border',
                                    value: '2px solid rgb(252, 253, 254)'
                                },
                                {
                                    property: 'padding',
                                    value: '24px 16px'
                                },
                                {
                                    property: 'font-weight',
                                    value: 700
                                },
                                {
                                    property: 'font-size',
                                    value: '16px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lusitana', serif"
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
                templateID: 6,
                themeID: 3,
                faces: [],
                elements: []
            }


        }

        return new tempViewModel();
    }
)