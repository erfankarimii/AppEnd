<template>
    <section id="contact-section" class="section bg-light">
        <div class="container">
            <h2 class="section-title">{{ shared.translate("Contact Us") }}</h2>
            <div class="row justify-content-center">
                <div class="col-lg-32">
                    <div class="contact-form">
                        <form @submit.prevent="submitForm">
                            <div class="mb-3">
                                <label for="contactName" class="form-label">{{ translate("Name") }}</label>
                                <input 
                                    type="text" 
                                    class="form-control" 
                                    id="contactName" 
                                    v-model="formData.name"
                                    :placeholder="translate('Your Name')"
                                    required
                                />
                            </div>
                            <div class="mb-3">
                                <label for="contactEmail" class="form-label">{{ translate("Email") }}</label>
                                <input 
                                    type="email" 
                                    class="form-control" 
                                    id="contactEmail" 
                                    v-model="formData.email"
                                    :placeholder="translate('Your Email')"
                                    required
                                />
                            </div>
                            <div class="mb-3">
                                <label for="contactMessage" class="form-label">{{ translate("Message") }}</label>
                                <textarea 
                                    class="form-control" 
                                    id="contactMessage" 
                                    rows="5"
                                    v-model="formData.message"
                                    :placeholder="translate('Your Message')"
                                    required
                                ></textarea>
                            </div>
                            <div class="text-center">
                                <button type="submit" class="btn btn-primary-custom btn-lg text-white" :disabled="submitting">
                                    <span v-if="!submitting">
                                        <i class="fa-solid fa-fw fa-paper-plane ms-2"></i>
                                        {{ translate("Send Message") }}
                                    </span>
                                    <span v-else>
                                        <span class="spinner-border spinner-border-sm ms-2" role="status"></span>
                                        {{ translate("Sending") }}...
                                    </span>
                                </button>
                            </div>
                            <div v-if="submitMessage" class="alert mt-3" :class="submitMessageType === 'success' ? 'alert-success' : 'alert-danger'">
                                {{ submitMessage }}
                            </div>
                        </form>
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
    formData: {
        name: "",
        email: "",
        message: ""
    },
    submitting: false,
    submitMessage: "",
    submitMessageType: "success"
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
        },
        submitForm() {
            _this.submitting = true;
            _this.submitMessage = "";
            
            if (typeof rpc === 'function') {
                rpc({
                    requests: [{ 
                        Method: "DefaultRepo.PublicMethods.SendContactMessage", 
                        Inputs: {
                            Name: _this.formData.name,
                            Email: _this.formData.email,
                            Message: _this.formData.message
                        }
                    }],
                    onDone: (responses) => {
                        if (responses && responses.length > 0 && responses[0].IsSucceeded) {
                            _this.submitMessage = _this.c.translate("Message sent successfully! We will contact you soon.");
                            _this.submitMessageType = "success";
                            _this.formData = { name: "", email: "", message: "" };
                        } else {
                            _this.submitMessage = _this.c.translate("Error sending message. Please try again.");
                            _this.submitMessageType = "danger";
                        }
                        _this.submitting = false;
                        
                        setTimeout(() => {
                            _this.submitMessage = "";
                        }, 5000);
                    },
                    onFail: (error) => {
                        console.error("Error sending message:", error);
                        _this.submitMessage = _this.c.translate("Error sending message. Please try again.");
                        _this.submitMessageType = "danger";
                        _this.submitting = false;
                    }
                });
            } else {
                console.error("rpcAEP is not available");
                _this.submitMessage = "Error: RPC function not available";
                _this.submitMessageType = "danger";
                _this.submitting = false;
            }
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
</style>

