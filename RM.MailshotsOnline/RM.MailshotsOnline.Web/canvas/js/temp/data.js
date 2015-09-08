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
                    name: 'Card Format',
                    faces: [
                        {
                            name:'Front',
                            side:'front',
                            width: 600,
                            height: 450,
                            default: true
                        },
                        {
                            name:'Back',
                            side:'front',
                            width: 600,
                            height: 450
                        },
                        {
                            name:'Inside',
                            side:'back',
                            width: 600,
                            height: 900
                        }
                    ]
                }
            ]

            this.templateData = [
                {
                    id: 5,
                    format_id: 2,
                    name: 'Card Template',
                    elements: [
                        {
                            name: 'fold',
                            face: 'Inside',
                            type: 'fold',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '449px'
                                },
                                {
                                    property: 'left',
                                    value: '0'
                                },
                                {
                                    property: 'right',
                                    value: '0'
                                }
                            ]
                        },
                        {
                            name: 'heading',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'heading',
                            layout: [
                            	{
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '20px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '280px'
                                },
                                {
                                    property: 'height',
                                    value: '125px'
                                }
                            ],
                            message: "This is your headline, it should grab the readers' attention quickly with an engaging message. Try to keep it short and punchy"
                        },
                        {
                            name: 'sub-heading',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'subheading',
                            layout: [
                            	{
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '150px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '280px'
                                },
                                {
                                    property: 'height',
                                    value: '60px'
                                },
                                {
                                	property: 'text-align',
                                	value: 'left'
                                }
                            ],
                            message: "This is your sub heading. Elaborate on your main heading here."
                        },
                        {
                            name: 'body',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'body',
                            layout: [
                            	{
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '225px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '280px'
                                },
                                {
                                    property: 'height',
                                    value: '100px'
                                },
                                {
                                	property: 'text-align',
                                	value: 'left'
                                }
                            ],
                            message: "Body copy placeholder message, to be confirmed"
                        },
                        {
                            name: 'cta',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'cta',
                            layout: [
                            	{
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '340px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '280px'
                                },
                                {
                                    property: 'height',
                                    value: '80px'
                                },
                                {
                                	property: 'text-align',
                                	value: 'left'
                                }
                            ],
                            message: "CTA copy placeholder message, to be confirmed"
                        },
                        {
                            name: 'logo',
                            face: 'Front',
                            type: 'image',
                            theme_class: 'logo',
                            layout: [
                            	{
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '0'
                                },
                                {
                                    property: 'left',
                                    value: '340px'
                                },
                                {
                                    property: 'width',
                                    value: '260px'
                                },
                                {
                                    property: 'height',
                                    value: '450px'
                                }
                            ],
                            message: "Choose a high quality image which helps convey your message to the reader."
                        },
                        {
                            name: 'Address',
                            face: 'Back',
                            type: 'noprint',
                            layout: [
                            	{
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '250px'
                                },
                                {
                                    property: 'left',
                                    value: '20px'
                                },
                                {
                                    property: 'width',
                                    value: '300px'
                                },
                                {
                                    property: 'height',
                                    value: '160px'
                                }
                            ]
                        },
                        {
                            name: 'Postage',
                            face: 'Back',
                            type: 'noprint',
                            layout: [
                            	{
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '0'
                                },
                                {
                                    property: 'right',
                                    value: '0'
                                },
                                {
                                    property: 'width',
                                    value: '200px'
                                },
                                {
                                    property: 'height',
                                    value: '100px'
                                }
                            ]
                        }
                    ]
                },
                {
                    id: 6,
                    format_id: 2,
                    name: 'Card Template 2',
                    elements: [
                        {
                            name: 'heading',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'heading-large',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '20px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '540px'
                                },
                                {
                                    property: 'height',
                                    value: '200px'
                                }
                            ],
                            message: "This is your headline, it should grab the readers' attention quickly with an engaging message. Try to keep it short and punchy"
                        },
                        {
                            name: 'sub-heading',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'subheading-large',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '240px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '540px'
                                },
                                {
                                    property: 'height',
                                    value: '60px'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                }
                            ],
                            message: "This is your sub heading. Elaborate on your main heading here."
                        },
                        {
                            name: 'body',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'body',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '320px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '260px'
                                },
                                {
                                    property: 'height',
                                    value: '100px'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                }
                            ],
                            message: "Body copy placeholder message, to be confirmed"
                        },
                        {
                            name: 'cta',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'cta-bigger',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '320px'
                                },
                                {
                                    property: 'left',
                                    value: '300px'
                                },
                                {
                                    property: 'width',
                                    value: '270px'
                                },
                                {
                                    property: 'height',
                                    value: '100px'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                }
                            ],
                            message: "CTA copy placeholder message, to be confirmed"
                        },
                        {
                            name: 'Address',
                            face: 'Back',
                            type: 'noprint',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '250px'
                                },
                                {
                                    property: 'left',
                                    value: '20px'
                                },
                                {
                                    property: 'width',
                                    value: '300px'
                                },
                                {
                                    property: 'height',
                                    value: '160px'
                                }
                            ]
                        },
                        {
                            name: 'Postage',
                            face: 'Back',
                            type: 'noprint',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '0'
                                },
                                {
                                    property: 'right',
                                    value: '0'
                                },
                                {
                                    property: 'width',
                                    value: '200px'
                                },
                                {
                                    property: 'height',
                                    value: '100px'
                                }
                            ]
                        }
                    ]
                },
                {
                    id: 7,
                    format_id: 2,
                    name: 'Card Template 3',
                    elements: [
                        {
                            name: 'heading',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'heading',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '320px'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '540px'
                                },
                                {
                                    property: 'height',
                                    value: '60px'
                                }
                            ],
                            message: "This is your headline, it should grab the readers' attention quickly with an engaging message. Try to keep it short and punchy"
                        },
                        {
                            name: 'sub-heading',
                            face: 'Front',
                            type: 'html',
                            theme_class: 'subheading-noline',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '390'
                                },
                                {
                                    property: 'left',
                                    value: '30px'
                                },
                                {
                                    property: 'width',
                                    value: '540px'
                                },
                                {
                                    property: 'height',
                                    value: '50px'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                }
                            ],
                            message: "This is your sub heading. Elaborate on your main heading here."
                        },
                        {
                            name: 'logo',
                            face: 'Front',
                            type: 'image',
                            theme_class: 'logo',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '0'
                                },
                                {
                                    property: 'left',
                                    value: '0'
                                },
                                {
                                    property: 'width',
                                    value: '600px'
                                },
                                {
                                    property: 'height',
                                    value: '300px'
                                },
                                {
                                    property: 'text-align',
                                    value: 'left'
                                }
                            ],
                            message: "Image copy placeholder message, to be confirmed"
                        },
                        {
                            name: 'Address',
                            face: 'Back',
                            type: 'noprint',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '250px'
                                },
                                {
                                    property: 'left',
                                    value: '20px'
                                },
                                {
                                    property: 'width',
                                    value: '300px'
                                },
                                {
                                    property: 'height',
                                    value: '160px'
                                }
                            ]
                        },
                        {
                            name: 'Postage',
                            face: 'Back',
                            type: 'noprint',
                            layout: [
                                {
                                    property: 'position',
                                    value: 'absolute'
                                },
                                {
                                    property: 'top',
                                    value: '0'
                                },
                                {
                                    property: 'right',
                                    value: '0'
                                },
                                {
                                    property: 'width',
                                    value: '200px'
                                },
                                {
                                    property: 'height',
                                    value: '100px'
                                }
                            ]
                        }
                    ]
                }
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
	                    			value: '#D4E5E6'
			                    },
                    		]
                    	},
                    	{
                    		name: 'Back',
                    		styles: [
                    			{
	                    			property: 'background-color',
	                    			value: '#D4E5E6'
			                    }
                    		]
                    	},
                        {
                            name: 'Inside',
                            styles: [
                                {
                                    property: 'background-color',
                                    value: '#D4E5E6'
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
                                	property: 'border',
                                	value: '2px dashed rgb(50, 132, 134)'
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
                formatID: 2,
                templateID: 5,
                themeID: 3,
                side: 'front',
                face: 'Front',
                faces: [],
                elements: [
                    {
                        name: 'heading',
                        content: 'Big headline goes here.',
                    },
                    {
                        name: 'sub-heading',
                        content: 'Sub-line with a bit more detail goes here.',
                    },
                    {
                        name: 'body',
                        content: "Far far away, behind the word mountains, far from the countries, there live the blind texts. Separated they live in right at the coast of the Semantics, a large language ocean.  It is a paradisematic country.",
                    },
                    {
                        name: 'cta',
                        content: 'Call to action! What do you want them to do?',
                    },
                ]
            }


        }

        return new tempViewModel();
    }
)