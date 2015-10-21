// temp data for use during development
define([],
    function() {

    	/**
    	 * View Model containing dummy data for use during development
    	 * @return {function}     viewModel
    	 */
        function tempViewModel() {

            this.formatData = [
                /*{
                    id: 2,
                    name: 'A5 Card',
                    faces: [
                        {
                            name:'Front',
                            side:'front',
                            width: 840, //210mm - ratio = 4 pixels per mm
                            height: 592, //148mm
                            default_face: true
                        },
                        {
                            name:'Back',
                            side:'back',
                            width: 840,
                            height: 592
                        }
                    ]
                },*/
                /*{
                    id: 4,
                    name: 'A4 Letter',
                    faces: [
                        {
                            name:'Letter',
                            side:'front',
                            width: 840,
                            height: 1184,
                            default_face: true
                        }
                    ]
                },*/
                {
                    id: 3,
                    name: 'Self Mailer',
                    faces: [
                        {
                            name:'Front',
                            side:'front',
                            width: 840,
                            height: 592,
                            default_face: true
                        },
                        {
                            name:'Back',
                            side:'front',
                            width: 840,
                            height: 592
                        },
                        {
                            name:'Inside',
                            side:'back',
                            width: 840,
                            height: 1184
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"440px"
                                },
                                {
                                    property:"height",
                                    value:"592px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"180px"
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
                                    value:"180px"
                                }
                            ]
                        },
                        {
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline_1",
                            title: "Subline",
                            message: "<ul><li>Introduce more detail about your product, service or offer</li><li>Further the message of your headline</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"370px"
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
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name:"cta",
                            face:"Front",
                            type:"html",
                            theme_class:"cta",
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"330px"
                                },
                                {
                                    property:"height",
                                    value:"85px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            title: "Highlight",
                            message: "<ul><li>Highlight an offer (e.g. 20% off, see inside) or an action (e.g. call now)</li><li>Consider adding a time limit (offer ends July) to increase urgency of a response</li></ul>",
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
                            ]
                        },
                        {
                            name:"back_logo",
                            face:"Back",
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"440px"
                                },
                                {
                                    property:"height",
                                    value:"592px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"180px"
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
                                    value:"180px"
                                }
                            ]
                        },
                        {
                            name:"subline",
                            face:"Front",
                            type:"html",
                            theme_class:"subline_1",
                            title: "Subline",
                            message: "<ul><li>Introduce more detail about your product, service or offer</li><li>Further the message of your headline</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"370px"
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
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name:"cta",
                            face:"Front",
                            type:"html",
                            theme_class:"cta",
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"330px"
                                },
                                {
                                    property:"height",
                                    value:"85px"
                                }
                            ]
                        },
                        {
                            name:"back_logo",
                            face:"Back",
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            title: "Highlight",
                            message: "<ul><li>Highlight an offer (e.g. 20% off, see inside) or an action (e.g. call now)</li><li>Consider adding a time limit (offer ends July) to increase urgency of a response</li></ul>",
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"back_logo",
                            face:"Back",
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"592px"
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
                                    value:"780px"
                                },
                                {
                                    property:"height",
                                    value:"592px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
                                }
                            ]
                        }
                    ]
                }*/

                // self mailer templates
                {
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"572px"
                                }
                            ]
                        },
                        {
                            name:"box",
                            face:"Inside",
                            type:"box",
                            theme_class:"complimentarybox",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"572px"
                                },
                                {
                                    property:"left",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"20px"
                                }
                            ]
                        },
                        {
                            name:"thinbox",
                            face:"Inside",
                            type:"box",
                            theme_class:"thintopborder",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"1060px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
                        {
                            name:"inside_headline",
                            face:"Inside",
                            type:"html",
                            theme_class:"headline_1",
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
                                    value:"760px"
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
                            theme_class:"subline_1",
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
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
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
                            theme_class:"cta",
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
                                    property:"right",
                                    value:"40px"
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
                            name:"inside_footer",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
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
                                    value:"540px"
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
                            type:"logo",
                            theme_class:"logo",
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
                                    property:"right",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"120px"
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
                                    value:"592px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            title: "Highlight",
                            message: "<ul><li>Highlight an offer (e.g. 20% off, see inside) or an action (e.g. call now)</li><li>Consider adding a time limit (offer ends July) to increase urgency of a response</li></ul>",
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            name:"box",
                            face:"Inside",
                            type:"box",
                            theme_class:"complimentarybox",
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
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"20px"
                                }
                            ]
                        },
                        {
                            name:"thinbox",
                            face:"Inside",
                            type:"box",
                            theme_class:"thintopborder",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"1060px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"25px"
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
                                    value:"560px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_1",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"725px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"560px"
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
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_2",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"725px"
                                },
                                {
                                    property:"left",
                                    value:"303px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"560px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_3",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"725px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"780px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_4",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"945px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"780px"
                                },
                                {
                                    property:"left",
                                    value:"303px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_5",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"945px"
                                },
                                {
                                    property:"left",
                                    value:"303px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"780px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_6",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"945px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                            theme_class:"headline_1",
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
                                    value:"760px"
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
                            theme_class:"subline_1",
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
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"270px"
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
                            theme_class:"cta",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"270px"
                                },
                                {
                                    property:"right",
                                    value:"40px"
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
                            name:"inside_footer",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
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
                                    value:"540px"
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
                            type:"logo",
                            theme_class:"logo",
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
                                    property:"right",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"120px"
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
                                    value:"592px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            title: "Highlight",
                            message: "<ul><li>Highlight an offer (e.g. 20% off, see inside) or an action (e.g. call now)</li><li>Consider adding a time limit (offer ends July) to increase urgency of a response</li></ul>",
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            name:"box",
                            face:"Inside",
                            type:"box",
                            theme_class:"complimentarybox",
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
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"20px"
                                }
                            ]
                        },
                        {
                            name:"thinbox",
                            face:"Inside",
                            type:"box",
                            theme_class:"thintopborder",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"1060px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"25px"
                                }
                            ]
                        },
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"500px"
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
                                    value:"700px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_1",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"865px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"700px"
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
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_2",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"865px"
                                },
                                {
                                    property:"left",
                                    value:"303px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"700px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_3",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"865px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                            theme_class:"headline_1",
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
                                    value:"370px"
                                },
                                {
                                    property:"height",
                                    value:"140px"
                                }
                            ]
                        },
                        {
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
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
                                    value:"430px"
                                },
                                {
                                    property:"width",
                                    value:"370px"
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
                            theme_class:"cta",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"bottom",
                                    value:"150px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"110px"
                                }
                            ]
                        },
                        {
                            name:"inside_footer",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
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
                                    value:"540px"
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
                            type:"logo",
                            theme_class:"logo",
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
                                    property:"right",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"120px"
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
                                    value:"592px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            title: "Highlight",
                            message: "<ul><li>Highlight an offer (e.g. 20% off, see inside) or an action (e.g. call now)</li><li>Consider adding a time limit (offer ends July) to increase urgency of a response</li></ul>",
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            name:"box",
                            face:"Inside",
                            type:"box",
                            theme_class:"complimentarybox",
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
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"20px"
                                }
                            ]
                        },
                        {
                            name:"thinbox",
                            face:"Inside",
                            type:"box",
                            theme_class:"thintopborder",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"1060px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"25px"
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
                                    value:"265px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_1",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
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
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"480px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_2",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"645px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                                    value:"695px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"image_caption_3",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"860px"
                                },
                                {
                                    property:"left",
                                    value:"566px"
                                },
                                {
                                    property:"width",
                                    value:"234px"
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
                            theme_class:"headline_1",
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
                                    value:"760px"
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
                            theme_class:"subline_1",
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
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"inside_body",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"265px"
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
                                    value:"634px"
                                }
                            ]
                        },
                        {
                            name:"inside_cta",
                            face:"Inside",
                            type:"html",
                            theme_class:"cta",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"bottom",
                                    value:"150px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"110px"
                                }
                            ]
                        },
                        {
                            name:"inside_footer",
                            face:"Inside",
                            type:"html",
                            theme_class:"body_1",
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
                                    value:"540px"
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
                            type:"logo",
                            theme_class:"logo",
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
                                    property:"right",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"120px"
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
                                    value:"592px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Front",
                            type:"html",
                            theme_class:"headline_1",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"445px"
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
                                    value:"125px"
                                }
                            ]
                        },
                        {
                            name:"offer",
                            face:"Front",
                            type:"roundel",
                            theme_class:"roundel",
                            title: "Highlight",
                            message: "<ul><li>Highlight an offer (e.g. 20% off, see inside) or an action (e.g. call now)</li><li>Consider adding a time limit (offer ends July) to increase urgency of a response</li></ul>",
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
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                                    value:"100px"
                                },
                                {
                                    property:"height",
                                    value:"50px"
                                }
                            ]
                        },
                        {
                            name:"back_headline",
                            face:"Back",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Continues the communication that your front started</li><li>Lead into main body of text</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
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
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"390px"
                                },
                                {
                                    property:"height",
                                    value:"70px"
                                }
                            ]
                        },
                        {
                            name: "footer",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            title: "Contact Details",
                            message: "<ul><li>Include the places you would like to be found </li><li>Physical addresses and websites can add a layer of trust</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"250px"
                                },
                                {
                                    property:"height",
                                    value:"40px"
                                }
                            ]
                        },
                        {
                            name: "return_address",
                            face:"Back",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"450px"
                                },
                                {
                                    property:"right",
                                    value:"70px"
                                },
                                {
                                    property:"width",
                                    value:"230px"
                                },
                                {
                                    property:"height",
                                    value:"60px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                            title: "Hero Image",
                            message: "<ul><li>Catch the eye of the reader</li><li>Work in conjunction and support your headline</li></ul>",
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"592px"
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
                                    value:"780px"
                                },
                                {
                                    property:"height",
                                    value:"592px"
                                }
                            ]
                        },
                        {
                            name: "Indicia",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name: "Tag Codemark",
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
                                    value:"160px"
                                },
                                {
                                    property:"right",
                                    value:"0"
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
                        },
                        {
                            name: "Route Codemark",
                            face:"Back",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"520px"
                                },
                                {
                                    property:"height",
                                    value:"72px"
                                }
                            ]
                        },
                        {
                            name: "",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"0"
                                },
                                {
                                    property:"width",
                                    value:"60px"
                                },
                                {
                                    property:"height",
                                    value:"320px"
                                }
                            ]
                        },
                        {
                            name: "Delivery Address",
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
                                    value:"200px"
                                },
                                {
                                    property:"right",
                                    value:"60px"
                                },
                                {
                                    property:"width",
                                    value:"240px"
                                },
                                {
                                    property:"height",
                                    value:"240px"
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"1184px"
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
                                    value:"592px"
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
                        }
                    ]
                }

                // letter templates
                /*{
                    id:14,
                    format_id:4,
                    name:"Letter Template 1",
                    elements:[
                        {
                            name:"logo",
                            face:"Letter",
                            type:"logo",
                            theme_class:"logo",
                            title: "Company Logo",
                            message: "<ul><li>Make sure it stands out against the background</li></ul>",
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
                            face:"Letter",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    value:"400px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Letter",
                            type:"html",
                            theme_class:"headline_2",
                            title: "Headline",
                            message: "<ul><li>Say what the product, service or purpose is, or the problem it solves</li><li>Be clear, bold and keep it short as possible - don’t be too clever</li><li>Try personalising by including the reader’s name</li></ul>",
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
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
                            title: "Body",
                            message: "<ul><li>Use short sentences, simple words and an active tone of voice</li><li>Use paragraph headings, bold and bullet points so it's easily scannable</li><li>Fill with customer-focused benefits and persuasive language</li></ul>",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"550px"
                                },
                                {
                                    property:"height",
                                    value:"620px"
                                }
                            ]
                        },
                        {
                            name:"return_address",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
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
                            name:"inside_cta",
                            face:"Letter",
                            type:"html",
                            theme_class:"cta",
                            title: "Call to Action",
                            message: "<ul><li>Tell the customer what you would like them to do (e.g. Call now)</li><li>Include your primary contact details for them to get in touch</li></ul>",
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"90px"
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
                            face:"Letter",
                            type:"logo",
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
                            face:"Letter",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    value:"400px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Letter",
                            type:"html",
                            theme_class:"headline_2",
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
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
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
                                    value:"40px"
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
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
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
                            name:"sign_off",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"990px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"550px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"signature",
                            face:"Letter",
                            type:"logo",
                            theme_class:"logo",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"900px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"inside_cta",
                            face:"Letter",
                            type:"html",
                            theme_class:"cta",
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"90px"
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
                            face:"Letter",
                            type:"logo",
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
                            face:"Letter",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    value:"400px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Letter",
                            type:"html",
                            theme_class:"headline_2",
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
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"550px"
                                },
                                {
                                    property:"height",
                                    value:"710px"
                                }
                            ]
                        },
                        {
                            name:"return_address",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
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
                        }
                    ]
                },
                {
                    id:17,
                    format_id:4,
                    name:"Letter Template 4",
                    elements:[
                        {
                            name:"logo",
                            face:"Letter",
                            type:"logo",
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
                            face:"Letter",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    value:"400px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                        {
                            name:"headline",
                            face:"Letter",
                            type:"html",
                            theme_class:"headline_2",
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
                                    value:"760px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"body",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
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
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"550px"
                                },
                                {
                                    property:"height",
                                    value:"540px"
                                }
                            ]
                        },
                        {
                            name:"return_address",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
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
                            name:"sign_off",
                            face:"Letter",
                            type:"html",
                            theme_class:"body_2",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"1080px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"550px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        },
                        {
                            name:"signature",
                            face:"Letter",
                            type:"logo",
                            theme_class:"logo",
                            layout:[
                                {
                                    property:"position",
                                    value:"absolute"
                                },
                                {
                                    property:"top",
                                    value:"990px"
                                },
                                {
                                    property:"left",
                                    value:"40px"
                                },
                                {
                                    property:"width",
                                    value:"300px"
                                },
                                {
                                    property:"height",
                                    value:"80px"
                                }
                            ]
                        }
                    ]
                },
                {
                    id:18,
                    format_id:4,
                    name:"Letter Template 5",
                    elements:[
                        {
                            name:"hero",
                            face:"Letter",
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
                                    value:"840px"
                                },
                                {
                                    property:"height",
                                    value:"1184"
                                }
                            ]
                        },
                        {
                            name: "Postage Info",
                            face:"Letter",
                            type:"noprint",
                            theme_class:"noprint",
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
                                    value:"400px"
                                },
                                {
                                    property:"height",
                                    value:"160px"
                                }
                            ]
                        },
                    ]
                }*/
            ]

            this.themeData = [
                {
                    id: 3,
                    name: 'Fashion',
                    faces: [
                        {
                            name: 'Letter',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#FFFFFF'
                                }
                            ]
                        },
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
	                    			value: '#FFFFFF'
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
                    colours: ['#4B4C4E', '#A0D0AA', '#FFFFFF', '#393939'],
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
                            name: 'complimentarybox',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#A0D0AA'
                                }
                            ]
                        },
                        {
                            name: 'thintopborder',
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
                            name: 'roundel',
                            font_sizes: ['18px', '19px', '20px', '21px', '22px', '23px', '24px', '25px', '26px', '27px', '28px', '29px', '30px', '31px', '32px', '33px', '34px', '35px', '36px', '37px', '38px', '39px', '40px', '41px', '42px', '43px', '44px', '45px', '46px', '48px', '50px', '52px'],
                            vertical_middle: true,
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
                            name: 'headline_1',
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
                                }
                            ]
                        },
                        {
                            name: 'subline_1',
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
                                }
                            ]
                        },
                        {
                            name: 'body_1',
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
                            name: 'headline_2',
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
                                }
                            ]
                        },
                        {
                            name: 'subline_2',
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
                                    value: '#4B4C4E'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Roboto', serif"
                                }
                            ]
                        },
                        {
                            name: 'body_2',
                            font_sizes: ['8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#393939'
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
                            vertical_middle: true,
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
                                	value: '0 20px'
                                },
                                {
                                	property: 'font-size',
                                	value: '26px'
                                },
			                    {
                                	property: 'font-family',
                                	value: "'Merriweather', serif"
                                },
                            ]
                        },
                    ]
                },
                {
                    id: 4,
                    name: 'Trusted / Traditional',
                    faces: [
                        {
                            name: 'Letter',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#FFFFFF'
                                }
                            ]
                        },
                    	{
                    		name: 'Front',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#37154D'
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
                                    value: '#37154D'
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
                    colours: ['#37154D', '#4F8436', '#393939', '#FFFFFF' ],
                    classes: [
                        {
                            name: 'complimentarybox',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#4F8436'
                                }
                            ]
                        },
                        {
                            name: 'thintopborder',
                            styles: [
                                {
                                    property: 'border-top',
                                    value: '2px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4F8436'
                                }
                            ]
                        },
                        {
                            name: 'roundel',
                            font_sizes: ['18px', '19px', '20px', '21px', '22px', '23px', '24px', '25px', '26px', '27px', '28px', '29px', '30px', '31px', '32px', '33px', '34px', '35px', '36px', '37px', '38px', '39px', '40px', '41px', '42px', '43px', '44px', '45px', '46px', '48px', '50px', '52px'],
                            vertical_middle: true,
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
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'background-color',
                                    value: '#37154D'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
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
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4F8436'
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
                            name: 'headline_1',
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
                            name: 'subline_1',
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
                                }
                            ]
                        },
                        {
                            name: 'body_1',
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
                                }
                            ]
                        },
                        {
                            name: 'headline_2',
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
                                    value: '#37154D'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                }
                            ]
                        },
                        {
                            name: 'subline_2',
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
                                    value: '#4F8436'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Lato', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'body_2',
                            font_sizes: ['8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#393939'
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
                                }
                            ]
                        },

                        {
                            name: 'cta',
                            font_sizes: ['10px', '12px', '14px', '17px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            vertical_middle: true,
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'background-color',
                                    value: '#37154D'
                                },
                                {
                                    property: 'border',
                                    value: '4px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#4F8436'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'padding',
                                    value: '0 20px'
                                },
                                {
                                    property: 'font-size',
                                    value: '28px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Domine', serif"
                                },
                            ]
                        },
                    ]
                },
                {
                    id: 5,
                    name: 'Approachable',
                    faces: [
                        {
                            name: 'Letter',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#FFFFFF'
                                }
                            ]
                        },
                    	{
                    		name: 'Front',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#FFFFFF'
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
                            name: 'Open Sans',
                            value: "'Open Sans', sans-serif"
                        },
                        {
                            name: 'Montserrat',
                            value: "'Montserrat', sans-serif"
                        }
                    ],
                    colours: ['#FFFFFF', '#009EE3', '#30AB62', '#393939' ],
                    classes: [
                        {
                            name: 'complimentarybox',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#009EE3'
                                }
                            ]
                        },
                        {
                            name: 'thintopborder',
                            styles: [
                                {
                                    property: 'border-top',
                                    value: '2px solid'
                                },
                                {
                                    property: 'border-color',
                                    value: '#009EE3'
                                }
                            ]
                        },
                        {
                            name: 'roundel',
                            font_sizes: ['18px', '19px', '20px', '21px', '22px', '23px', '24px', '25px', '26px', '27px', '28px', '29px', '30px', '31px', '32px', '33px', '34px', '35px', '36px', '37px', '38px', '39px', '40px', '41px', '42px', '43px', '44px', '45px', '46px', '48px', '50px', '52px'],
                            vertical_middle: true,
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
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'background-color',
                                    value: '#009EE3'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Montserrat', sans-serif"
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
                            name: 'headline_1',
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
                                    value: '#30AB62'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Montserrat', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'subline_1',
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
                                    value: '#009EE3'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Open Sans', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'body_1',
                            font_sizes: ['7px', '8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#393939'
                                },
                                {
                                    property: 'font-size',
                                    value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Open Sans', sans-serif"
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                }
                            ]
                        },
                        {
                            name: 'headline_2',
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
                                    value: '#30AB62'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Montserrat', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'subline_2',
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
                                    value: '#009EE3'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Open Sans', sans-serif"
                                }
                            ]
                        },
                        {
                            name: 'body_2',
                            font_sizes: ['7px', '8px', '9px', '10px', '11px', '12px', '13px', '14px', '15px', '18px', '20px', '22px', '24px', '26px', '28px', '30px', '32px', '34px', '36px'],
                            styles: [
                                {
                                    property: 'color',
                                    value: '#393939'
                                },
                                {
                                    property: 'font-size',
                                    value: '15px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Open Sans', sans-serif"
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
                            vertical_middle: true,
                            styles: [
                                {
                                    property: 'color',
                                    value: '#FFFFFF'
                                },
                                {
                                    property: 'background-color',
                                    value: '#009ee3'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                },
                                {
                                    property: 'padding',
                                    value: '0 20px'
                                },
                                {
                                    property: 'font-size',
                                    value: '28px'
                                },
                                {
                                    property: 'font-family',
                                    value: "'Montserrat', sans-serif"
                                },
                            ]
                        },
                    ]
                }
            ]

            this.userData = {
                formatID: 3,
                templateID: 10,
                themeID: 3,
                faces: [],
                elements: []
            }


        }

        return new tempViewModel();
    }
)