require.config(
{
    baseUrl: "/canvas/js/",
    paths:{
        jquery: "vendor/jquery/dist/jquery.min",
        knockout: "vendor/knockout/dist/knockout",
        komapping: "vendor/knockout-mapping/build/output/knockout.mapping-latest",
        spectrum: "vendor/spectrum/spectrum",
        domReady: "vendor/requirejs-domready/domReady",
        text: "vendor/requirejs-text/text",
        pointerevents: 'custom_bindings/pointer-events',
        koeditable: 'custom_bindings/ko-editable',
        koelement: 'custom_bindings/ko-element',
        kospectrum: 'custom_bindings/ko-spectrum',
        kofile: 'custom_bindings/ko-file',
        templates: "templates/"
    },
    shim: {
        komapping: {
            deps: ['knockout'],
            exports: 'komapping'
        },
        spectrum: {
            deps: ['jquery'],
            exports: 'spectrum'
        }
    },
    urlArgs: "v=" +  (new Date()).getTime()
});