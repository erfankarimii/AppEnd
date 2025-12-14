<template>
    <section id="features-section" class="section bg-white">
        <div class="container">
            <h2 class="section-title">{{ translate("Features") }}</h2>
            <div class="row g-4">
                <div class="col-24" v-for="(feature, index) in features" :key="index">
                    <div class="card feature-card h-100">
                        <div class="card-body text-center">
                            <div class="feature-icon">
                                <i :class="feature.icon"></i>
                            </div>
                            <h5 class="card-title">{{ translate(feature.title) }}</h5>
                            <p class="card-text text-muted">{{ translate(feature.description) }}</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
let _this = { 
    cid: "", 
    c: null,
    features: [
        {
            icon: "fa-solid fa-fw fa-code-branch",
            title: "Open Source",
            description: "Completely open source and free to use"
        },
        {
            icon: "fa-solid fa-fw fa-graduation-cap",
            title: "Easy Learning",
            description: "Low learning curve, easy to get started"
        },
        {
            icon: "fa-solid fa-fw fa-bolt",
            title: "Dynamic Code",
            description: "Dynamic code compilation and execution"
        },
        {
            icon: "fa-solid fa-fw fa-palette",
            title: "Template Engine",
            description: "Automatic UI generation from templates"
        },
        {
            icon: "fa-solid fa-fw fa-layer-group",
            title: "Full Stack",
            description: "Complete full-stack application platform"
        },
        {
            icon: "fa-solid fa-fw fa-rocket",
            title: "Rapid Development",
            description: "Build applications quickly with RAD tools"
        }
    ]
};

export default {
    setup(props) { 
        _this.cid = props['cid']; 
    },
    data() { 
        return _this; 
    },
    created() { 
        _this.c = this; 
    },
    mounted() { },
    methods: {
        translate(key) {
            if (typeof shared !== 'undefined' && typeof shared.translate === 'function') {
                return shared.translate(key);
            }
            return key;
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
</style>

